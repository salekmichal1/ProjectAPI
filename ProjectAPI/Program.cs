using ProjectAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using ProjectAPI;
using AutoMapper;
using FluentValidation;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Mapowanie));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

///<summary>
/// 
/// </summary>
app.MapGet("/api/product", (ILogger<Program> _logger) => {
    APIStatus odpowiedz = new();

    _logger.Log(LogLevel.Information, "Getting all Products");

    odpowiedz.Rezulstat = ProduktySD.produktyLista;
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;
    
    return Results.Ok(odpowiedz);

}).WithName("GetAllProducts").Produces<APIStatus>(200);

////////////////////////////////////////////////////

app.MapGet("/api/product/{id:int}", (ILogger<Program> _logger, int id) => {

    APIStatus odpowiedz = new();

    odpowiedz.Rezulstat = ProduktySD.produktyLista.FirstOrDefault(u => u.Id == id);
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;

}).WithName("GetProductById").Produces<APIStatus>(200);

//////////////////////////////////////////////////////////

app.MapPost("/api/product", async (IMapper _mapper,
    IValidator <StworzProduktDTO> _validation, [FromBody] StworzProduktDTO stworzProduktDTO) => {

        APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };


        var validationResult = await _validation.ValidateAsync(stworzProduktDTO);

    if (!validationResult.IsValid)
    {
        odpowiedz.Bledy.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(odpowiedz);
    }

    if (ProduktySD.produktyLista.FirstOrDefault(u => u.Nazwa.ToLower() == stworzProduktDTO.Nazwa.ToLower()) != null)
    {
        odpowiedz.Bledy.Add("Produkt juz istnieje");
        return Results.BadRequest(odpowiedz);
    }

    Produkt produkt = _mapper.Map<Produkt>(stworzProduktDTO);


    produkt.Id = ProduktySD.produktyLista.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

        ProduktySD.produktyLista.Add(produkt);

    ProduktDTO produktDTO = _mapper.Map<ProduktDTO>(produkt);

        odpowiedz.Rezulstat = produktDTO;
        odpowiedz.Akceptacja = true;
        odpowiedz.KodStanu = HttpStatusCode.Created;
        return Results.Ok(odpowiedz);

    }).WithName("CreateProduct").Accepts<StworzProduktDTO>("application/json").Produces<ProduktDTO>(201).Produces(400);

//////////////////////////

app.MapPut("/api/product", async (IMapper _mapper,
    IValidator<AktualizujProduktDTO> _validation, [FromBody] AktualizujProduktDTO aktualizujProduktDTO) =>
{

    APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };

    var validationResult = await _validation.ValidateAsync(aktualizujProduktDTO);
    if (!validationResult.IsValid)
    {
        odpowiedz.Bledy.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(odpowiedz);
    }

    Produkt produktPrzedAktualizacja = ProduktySD.produktyLista.FirstOrDefault(u => u.Id == aktualizujProduktDTO.Id);
    produktPrzedAktualizacja.Dostepny = aktualizujProduktDTO.Dostepny;
    produktPrzedAktualizacja.Nazwa = aktualizujProduktDTO.Nazwa;
    produktPrzedAktualizacja.Cena = aktualizujProduktDTO.Cena;
    produktPrzedAktualizacja.Ilosc = aktualizujProduktDTO.Ilosc;
    produktPrzedAktualizacja.Zaktualizowany = DateTime.Now;

    odpowiedz.Rezulstat = _mapper.Map<ProduktDTO>(produktPrzedAktualizacja); ;
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;

    return Results.Ok(odpowiedz);
}).WithName("UpdateProduct")
    .Accepts<AktualizujProduktDTO>("application/json").Produces<APIStatus>(200).Produces(400);

app.MapDelete("/api/product/{id:int}", (int id) => {

});

app.UseHttpsRedirection();

app.Run();
