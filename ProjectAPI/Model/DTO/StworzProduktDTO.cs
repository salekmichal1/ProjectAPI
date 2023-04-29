using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Model.DTO
{
    public class StworzProduktDTO
    {
        public string Nazwa { get; set; }
        public double Cena { get; set; }
        public int Ilosc { get; set; }
        public bool Dostepny { get; set; }
    }
}
