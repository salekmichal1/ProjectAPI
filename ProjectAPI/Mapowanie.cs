using ProjectAPI.Model.DTO;
using ProjectAPI.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace ProjectAPI
{
    /// <summary>
    /// Klasa impelmentujaca kopiwoeanie danych z klas tabel 
    /// </summary>
    public class Mapowanie : Profile
    {
        public Mapowanie()
        {
            CreateMap<Produkt, StworzProduktDTO>().ReverseMap();
            CreateMap<Produkt, AktualizujProduktDTO>().ReverseMap();
            CreateMap<Produkt, ProduktDTO>().ReverseMap();
            CreateMap<Urzytkownik, UrzytkownikDTO>().ReverseMap();
            CreateMap<ApplicationUser, UrzytkownikDTO>().ReverseMap();
        }
    }
}
