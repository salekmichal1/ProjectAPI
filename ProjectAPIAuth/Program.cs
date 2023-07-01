using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectAPIAuth.Model;
using ProjectAPIAuth.Data;
using System.Text;
using ProjectAPIAuth.Repository;
using ProjectAPIAuth;
using ProjectAPIAuth.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(Mapping));

builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(option => {
//    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description =
//             "Wpisz 'Bearer' [spacja] a natêpnie token uwierzytleniania\r\n\r\n" +
//             "\"Bearer 123456\"",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });
//    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
//                {
//                    {
//                        new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            },
//                            Scheme = "oauth2",
//                            Name = "Bearer",
//                            In = ParameterLocation.Header,

//                        },
//                        new List<string>()
//                    }
//                });


//});

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
//            builder.Configuration.GetValue<string>("ApiSettings:Secret"))),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});

builder.Services.AddScoped<IAuthorizationRepo, AuthorizationRepo>();


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

app.AuthorizationEndpointsConfiguration();

app.UseHttpsRedirection();

app.Run();

