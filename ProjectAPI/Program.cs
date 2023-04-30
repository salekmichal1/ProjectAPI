using ProjectAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using ProjectAPI;
using AutoMapper;
using FluentValidation;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.MapGet("/api/product", async (ApplicationDbContext _dbContext, ILogger<Program> _logger) => {

    APIStatus odpowiedz = new();

    _logger.Log(LogLevel.Information, "Getting all Products");

    odpowiedz.Rezulstat = _dbContext.Produkts;
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;
    
    return Results.Ok(odpowiedz);

}).WithName("GetAllProducts").Produces<APIStatus>(200);

////////////////////////////////////////////////////

app.MapGet("/api/product/{id:int}", async (ApplicationDbContext _dbContext, ILogger < Program> _logger, int id) => {

    APIStatus odpowiedz = new();

    odpowiedz.Rezulstat = await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Id == id);
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;

}).WithName("GetProductById").Produces<APIStatus>(200);

//////////////////////////////////////////////////////////

app.MapPost("/api/product", async (ApplicationDbContext _dbContext, IMapper _mapper,
    IValidator <StworzProduktDTO> _validation, [FromBody] StworzProduktDTO stworzProduktDTO) => {

        APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };


        var validationResult = await _validation.ValidateAsync(stworzProduktDTO);

    if (!validationResult.IsValid)
    {
        odpowiedz.Bledy.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(odpowiedz);
    }

    if(_dbContext.Produkts.FirstOrDefault(u => u.Nazwa.ToLower() == stworzProduktDTO.Nazwa.ToLower()) != null)
    {
        odpowiedz.Bledy.Add("Produkt juz istnieje");
        return Results.BadRequest(odpowiedz);
    }

    Produkt produkt = _mapper.Map<Produkt>(stworzProduktDTO);

        _dbContext.Produkts.Add(produkt);
        await _dbContext.SaveChangesAsync();
        ProduktDTO produktDTO = _mapper.Map<ProduktDTO>(produkt);


        odpowiedz.Rezulstat = produktDTO;
        odpowiedz.Akceptacja = true;
        odpowiedz.KodStanu = HttpStatusCode.Created;
        return Results.Ok(odpowiedz);

    }).WithName("CreateProduct").Accepts<StworzProduktDTO>("application/json").Produces<ProduktDTO>(201).Produces(400);

//////////////////////////

app.MapPut("/api/product", async (ApplicationDbContext _dbContext, IMapper _mapper,
    IValidator<AktualizujProduktDTO> _validation, [FromBody] AktualizujProduktDTO aktualizujProduktDTO) =>
{

    APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };

    var validationResult = await _validation.ValidateAsync(aktualizujProduktDTO);
    if (!validationResult.IsValid)
    {
        odpowiedz.Bledy.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(odpowiedz);
    }

    Produkt produktPrzedAktualizacja = await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Id == aktualizujProduktDTO.Id);
    produktPrzedAktualizacja.Dostepny = aktualizujProduktDTO.Dostepny;
    produktPrzedAktualizacja.Nazwa = aktualizujProduktDTO.Nazwa;
    produktPrzedAktualizacja.Cena = aktualizujProduktDTO.Cena;
    produktPrzedAktualizacja.Ilosc = aktualizujProduktDTO.Ilosc;
    produktPrzedAktualizacja.Zaktualizowany = DateTime.Now;

    await _dbContext.SaveChangesAsync();

    odpowiedz.Rezulstat = _mapper.Map<ProduktDTO>(produktPrzedAktualizacja); ;
    odpowiedz.Akceptacja = true;
    odpowiedz.KodStanu = HttpStatusCode.OK;

    return Results.Ok(odpowiedz);
}).WithName("UpdateProduct")
    .Accepts<AktualizujProduktDTO>("application/json").Produces<APIStatus>(200).Produces(400);

///////////////////////////////////////

app.MapDelete("/api/product/{id:int}", async (ApplicationDbContext _dbContext, int id) => {

    APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };


    Produkt produktSD = await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Id == id);
    if (produktSD != null)
    {
        _dbContext.Produkts.Remove(produktSD);
        await _dbContext.SaveChangesAsync();
        odpowiedz.Akceptacja = true;
        odpowiedz.KodStanu = HttpStatusCode.NoContent;
        return Results.Ok(odpowiedz);
    }
    else
    {
        odpowiedz.Bledy.Add("Invalid Id");
        return Results.BadRequest(odpowiedz);
    }
});

app.UseHttpsRedirection();

app.Run();
