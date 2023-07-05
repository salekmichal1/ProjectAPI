using ProjectAPIOrder;
using FluentValidation;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ProjectAPIOrder.Data;
using ProjectAPIOrder.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Kofiguracja swaggera pod weryfikacje u¿ytkownika
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
             "Wpisz 'Bearer' [spacja] a natêpnie token uwierzytleniania\r\n\r\n" +
             "\"Bearer 123456\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });


});

//builder.Services.AddSwaggerGen();

// Wstrzykiwanie Repozytorium


//builder.Services.AddScoped<IAuthorizationRepo, AuthorizationRepo>();

// £¹czenie z baz¹ danych
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Mapping));

builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddHttpClient("ProjectAPI", u => u.BaseAddress = new Uri(builder.Configuration["ServicesUrls:ProjecttAPI"]));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

///Kofiufuracja uwierzytelniania u¿ytkownika
///
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            builder.Configuration.GetValue<string>("ApiSettings:Secret"))),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

///Dodanie autoryzacji u¿ytkownika
///
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Wywo³anie 
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
