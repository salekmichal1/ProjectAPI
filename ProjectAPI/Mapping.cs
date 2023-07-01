using ProjectAPI.Model.DTO;
using ProjectAPI.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace ProjectAPI
{
    /// <summary>
    /// Klasa impelmentujaca kopiowanie danych z klas tabel 
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            //CreateMap<User, UserDTO>().ReverseMap();
            //CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
