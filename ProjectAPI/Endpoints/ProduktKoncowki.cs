using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Net;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using ProjectAPI.Repozytorium;
using Microsoft.AspNetCore.Authorization;

namespace ProjectAPI.Endpoints
{
    public static class ProduktKoncowki
    {
        public static void KonfiguracjaKoncowekProduktu(this WebApplication app)
        {

            app.MapGet("/api/product", GetAllProduct).WithName("GetAllProducts").Produces<APIStatus>(200).RequireAuthorization("Admin");

            app.MapGet("/api/product/{id:int}", GetProduct).WithName("GetProduct").Produces<APIStatus>(200);

            app.MapPost("/api/product", CreateProduct).WithName("CreateProduct").Accepts<StworzProduktDTO>("application/json").Produces<APIStatus>(201).Produces(400);

            app.MapPut("/api/product", UpdateProduct).WithName("UpdateProduct")
                .Accepts<AktualizujProduktDTO>("application/json").Produces<APIStatus>(200).Produces(400);

            app.MapDelete("/api/product/{id:int}", DeleteProduct);
        }

        private async static Task<IResult> GetProduct(IProduktRepo _produktRepo, ILogger<Program> _logger, int id)
        {
            Produkt produktSD = await _produktRepo.GetAsync(id);

            APIStatus odpowiedz = new();

            if (produktSD != null)
            {
                odpowiedz.Rezulstat = await _produktRepo.GetAsync(id);
                odpowiedz.Akceptacja = true;
                odpowiedz.KodStanu = HttpStatusCode.OK;

                return Results.Ok(odpowiedz);
            }
            else
            {
                odpowiedz.Bledy.Add("Nieprawidlowe Id");

                return Results.BadRequest(odpowiedz);
            }
        }

        [Authorize]
        private async static Task<IResult> CreateProduct(IProduktRepo _produktRepo, IMapper _mapper,
                IValidator<StworzProduktDTO> _walidacja, [FromBody] StworzProduktDTO stworzProduktDTO)
        {
            APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };

            var walidacjaRezultatu = await _walidacja.ValidateAsync(stworzProduktDTO);

            if (!walidacjaRezultatu.IsValid)
            {
                odpowiedz.Bledy.Add(walidacjaRezultatu.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(odpowiedz);
            }
            if (_produktRepo.GetAsync(stworzProduktDTO.Nazwa).GetAwaiter().GetResult() != null)
            {
                odpowiedz.Bledy.Add("Nazwa produktu juz istnieje");
                return Results.BadRequest(odpowiedz);
            }

            Produkt produkt = _mapper.Map<Produkt>(stworzProduktDTO);


            await _produktRepo.CreateAsync(produkt);
            await _produktRepo.SaveAsync();
            ProduktDTO produktDTO = _mapper.Map<ProduktDTO>(produkt);


            odpowiedz.Rezulstat = produktDTO;
            odpowiedz.Akceptacja = true;
            odpowiedz.KodStanu = HttpStatusCode.Created;

            return Results.Ok(odpowiedz);
        }

        [Authorize]
        private async static Task<IResult> UpdateProduct(IProduktRepo _produktRepo, IMapper _mapper,
                IValidator<AktualizujProduktDTO> _walidacja, [FromBody] AktualizujProduktDTO aktualizujProduktDTO)
        {
            APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };

            var walidacjaRezultatu = await _walidacja.ValidateAsync(aktualizujProduktDTO);
            if (!walidacjaRezultatu.IsValid)
            {
                odpowiedz.Bledy.Add(walidacjaRezultatu.Errors.FirstOrDefault().ToString());

                return Results.BadRequest(odpowiedz);
            }


            await _produktRepo.UpdateAsync(_mapper.Map<Produkt>(aktualizujProduktDTO));
            await _produktRepo.SaveAsync();

            odpowiedz.Rezulstat = _mapper.Map<ProduktDTO>(await _produktRepo.GetAsync(aktualizujProduktDTO.Id)); ;
            odpowiedz.Akceptacja = true;
            odpowiedz.KodStanu = HttpStatusCode.OK;

            return Results.Ok(odpowiedz);
        }

        [Authorize]
        private async static Task<IResult> DeleteProduct(IProduktRepo _produktRepo, int id)
        {
            APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };


            Produkt produktSD = await _produktRepo.GetAsync(id);
            if (produktSD != null)
            {
                await _produktRepo.RemoveAsync(produktSD);
                await _produktRepo.SaveAsync();
                odpowiedz.Akceptacja = true;
                odpowiedz.KodStanu = HttpStatusCode.NoContent;

                return Results.Ok(odpowiedz);
            }
            else
            {
                odpowiedz.Bledy.Add("Nieprawidlowe Id");

                return Results.BadRequest(odpowiedz);
            }
        }

        private async static Task<IResult> GetAllProduct(IProduktRepo _produktRepo, ILogger<Program> _logger)
        {
            APIStatus odpowiedz = new();
            _logger.Log(LogLevel.Information, "Pobierz wszystkie produkty");
            odpowiedz.Rezulstat = await _produktRepo.GetAllAsync();
            odpowiedz.Akceptacja = true;
            odpowiedz.KodStanu = HttpStatusCode.OK;

            return Results.Ok(odpowiedz);
        }
    }
}
