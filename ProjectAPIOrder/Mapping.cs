using ProjectAPIOrder.Model.DTO;
using ProjectAPIOrder.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace ProjectAPIOrder
{
    /// <summary>
    /// Klasa impelmentujaca kopiowanie danych z klas tabel 
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDTO>().ReverseMap();
        }
    }
}
