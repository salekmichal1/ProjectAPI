using AutoMapper;
using ProjectAPIAuth.Model.DTO;
using ProjectAPIAuth.Model;

namespace ProjectAPIAuth
{
    /// <summary>
    /// Klasa impelmentujaca kopiowanie danych z klas tabel 
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
