using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;
using ProjectAPI.Repozytorium;
using System.Net;

namespace ProjectAPI.Endpoints
{
    /// <summary>
    /// Klasa edpointów obsługujących autorayzacje użytkowników
    /// </summary>
    public static class AutoryzacjaKoncowki
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {

            app.MapPost("/api/login", Login).WithName("Login").Accepts<ProsbaLogowaniaDTO>("application/json")
                .Produces<APIStatus>(200).Produces(400);

            app.MapPost("/api/register", Register).WithName("Register").Accepts<ProsbaRejestracjiDTO>("application/json")
                .Produces<APIStatus>(200).Produces(400);
        }


        private async static Task<IResult> Register(IAutoryzacjaRepo _autoRepo,
            [FromBody] ProsbaRejestracjiDTO model)
        {
            APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };


            bool ifUserNameisUnique = _autoRepo.IsUniqueUser(model.NazwaUrzytkownika);
            if (!ifUserNameisUnique)
            {
                odpowiedz.Bledy.Add("Urzytkownik juz istnieje");
                return Results.BadRequest(odpowiedz);
            }
            var rejestracjaOdpowiedz = await _autoRepo.Register(model);

            if (rejestracjaOdpowiedz == null || string.IsNullOrEmpty(rejestracjaOdpowiedz.NazwaUrzytkownika))
            {

                return Results.BadRequest(odpowiedz);
            }

            odpowiedz.Akceptacja = true;
            odpowiedz.KodStanu = HttpStatusCode.OK;
            return Results.Ok(odpowiedz);

        }

        private async static Task<IResult> Login(IAutoryzacjaRepo _autoRepo,
           [FromBody] ProsbaLogowaniaDTO model)
        {
            APIStatus odpowiedz = new() { Akceptacja = false, KodStanu = HttpStatusCode.BadRequest };

            var logowanieOdpowiedz = await _autoRepo.Login(model);

            if (logowanieOdpowiedz == null)
            {
                odpowiedz.Bledy.Add("Nazwa lub haslo nieprawidlowe");
                return Results.BadRequest(odpowiedz);
            }
            odpowiedz.Rezulstat = logowanieOdpowiedz;
            odpowiedz.Akceptacja = true;
            odpowiedz.KodStanu = HttpStatusCode.OK;
            return Results.Ok(odpowiedz);

        }
    }
}
