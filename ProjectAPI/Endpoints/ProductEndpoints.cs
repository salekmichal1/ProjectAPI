using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Net;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using ProjectAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ProjectAPI.Endpoints
{
    /// <summary>
    /// Klasa edpointów obsługujących działania na produktach
    /// </summary>
    public static class ProductEndpoints
    {
        public static void ProductEndpointsConfiguration(this WebApplication app)
        {

            app.MapGet("/api/product", GetAllProduct).WithName("GetAllProducts").Produces<APIStatus>(200);

            app.MapGet("/api/product/{id:int}", GetProduct).WithName("GetProduct").Produces<APIStatus>(200);

            app.MapPost("/api/product", CreateProduct).WithName("CreateProduct").Accepts<CreateProductDTO>("application/json").Produces<APIStatus>(201).Produces(400).RequireAuthorization("Admin");

            app.MapPut("/api/product", UpdateProduct).WithName("UpdateProduct")
                .Accepts<UpdateProductDTO>("application/json").Produces<APIStatus>(200).Produces(400).RequireAuthorization("Admin");

            app.MapDelete("/api/product/{id:int}", DeleteProduct).RequireAuthorization("Admin");
        }

        private async static Task<IResult> GetProduct(IProductRepo _productRepo, ILogger<Program> _logger, int id)
        {
            Product productSD = await _productRepo.GetAsync(id);

            APIStatus response = new();

            if (productSD != null)
            {
                response.Result = await _productRepo.GetAsync(id);
                response.Accept = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Ok(response);
            }
            else
            {
                response.Errors.Add("Nieprawidlowe Id");

                return Results.BadRequest(response);
            }
        }

        [Authorize]
        private async static Task<IResult> CreateProduct(IProductRepo _productRepo, IMapper _mapper,
                IValidator<CreateProductDTO> _validator, [FromBody] CreateProductDTO createProductDTO)
        {
            APIStatus resposne = new() { Accept = false, StatusCode = HttpStatusCode.BadRequest };

            var resultValidation = await _validator.ValidateAsync(createProductDTO);

            if (!resultValidation.IsValid)
            {
                resposne.Errors.Add(resultValidation.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(resposne);
            }
            if (_productRepo.GetAsync(createProductDTO.Name).GetAwaiter().GetResult() != null)
            {
                resposne.Errors.Add("Nazwa produktu juz istnieje");
                return Results.BadRequest(resposne);
            }

            Product product = _mapper.Map<Product>(createProductDTO);


            await _productRepo.CreateAsync(product);
            await _productRepo.SaveAsync();
            ProductDTO productDTO = _mapper.Map<ProductDTO>(product);


            resposne.Result = productDTO;
            resposne.Accept = true;
            resposne.StatusCode = HttpStatusCode.Created;

            return Results.Ok(resposne);
        }

        [Authorize]
        private async static Task<IResult> UpdateProduct(IProductRepo _produktRepo, IMapper _mapper,
                IValidator<UpdateProductDTO> _validator, [FromBody] UpdateProductDTO updateProductDTO)
        {
            APIStatus resposne = new() { Accept = false, StatusCode = HttpStatusCode.BadRequest };

            var resultValidation = await _validator.ValidateAsync(updateProductDTO);
            if (!resultValidation.IsValid)
            {
                resposne.Errors.Add(resultValidation.Errors.FirstOrDefault().ToString());

                return Results.BadRequest(resposne);
            }


            await _produktRepo.UpdateAsync(_mapper.Map<Product>(updateProductDTO));
            await _produktRepo.SaveAsync();

            resposne.Result = _mapper.Map<ProductDTO>(await _produktRepo.GetAsync(updateProductDTO.Id)); ;
            resposne.Accept = true;
            resposne.StatusCode = HttpStatusCode.OK;

            return Results.Ok(resposne);
        }

        [Authorize]
        private async static Task<IResult> DeleteProduct(IProductRepo _productRepo, int id)
        {
            APIStatus resposne = new() { Accept = false, StatusCode = HttpStatusCode.BadRequest };


            Product productSD = await _productRepo.GetAsync(id);
            if (productSD != null)
            {
                await _productRepo.RemoveAsync(productSD);
                await _productRepo.SaveAsync();
                resposne.Accept = true;
                resposne.StatusCode = HttpStatusCode.NoContent;

                return Results.Ok(resposne);
            }
            else
            {
                resposne.Errors.Add("Nieprawidlowe Id");

                return Results.BadRequest(resposne);
            }
        }

        private async static Task<IResult> GetAllProduct(IProductRepo _productRepo, ILogger<Program> _logger)
        {
            APIStatus response = new();
            _logger.Log(LogLevel.Information, "Pobierz wszystkie produkty");
            response.Result = await _productRepo.GetAllAsync();
            response.Accept = true;
            response.StatusCode = HttpStatusCode.OK;

            return Results.Ok(response);
        }
    }
}
