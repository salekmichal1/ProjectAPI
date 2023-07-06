using Microsoft.AspNetCore.Mvc;
using ProjectAPIAuth.Model;
using ProjectAPIAuth.Model.DTO;
using ProjectAPIAuth.Repository;
using System.Net;

namespace ProjectAPIAuth.Endpoints
{
    /// <summary>
    /// Klasa edpointów obsługujących autorayzacje użytkowników
    /// </summary>
    public static class AuthorizationEndpoints
    {
        /// <summary>
        /// Konfiguracja endpointów
        /// </summary>
        /// <param name="app"></param>
        public static void AuthorizationEndpointsConfiguration(this WebApplication app)
        {

            app.MapPost("/api/auth/login", Login).WithName("Login").Accepts<LoginRequestDTO>("application/json")
                .Produces<APIStatus>(200).Produces(400);

            app.MapPost("/api/auth/register", Register).WithName("Register").Accepts<RegisterRequestDTO>("application/json")
                .Produces<APIStatus>(200).Produces(400);
        }

        /// <summary>
        /// Metoda odpowiadająca za rejestracja użytkownika
        /// </summary>
        /// <param name="_autoRepo"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async static Task<IResult> Register(IAuthorizationRepo _autoRepo, [FromBody] RegisterRequestDTO model)
        {
            APIStatus response = new() { Accept = false, StatusCode = HttpStatusCode.BadRequest };


            bool ifUserNameisUnique = _autoRepo.IsUniqueUser(model.UserName);
            if (!ifUserNameisUnique)
            {
                response.Errors.Add("Urzytkownik juz istnieje");
                return Results.BadRequest(response);
            }
            var registrationResposne = await _autoRepo.Register(model);

            if (registrationResposne == null || string.IsNullOrEmpty(registrationResposne.UserName))
            {

                return Results.BadRequest(response);
            }

            response.Accept = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);

        }

        /// <summary>
        /// Metoda odpowiadająca za logowanie użytkownika
        /// </summary>
        /// <param name="_autoRepo"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async static Task<IResult> Login(IAuthorizationRepo _autoRepo, [FromBody] LoginRequestDTO model)
        {
            APIStatus response = new() { Accept = false, StatusCode = HttpStatusCode.BadRequest };

            var loginResposne = await _autoRepo.Login(model);

            if (loginResposne == null)
            {
                response.Errors.Add("Nazwa lub haslo nieprawidlowe");
                return Results.BadRequest(response);
            }
            response.Result = loginResposne;
            response.Accept = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);

        }
    }
}
