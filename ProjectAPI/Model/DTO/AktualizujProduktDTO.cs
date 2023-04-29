namespace ProjectAPI.Model.DTO
{
    public class AktualizujProduktDTO
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public double Cena { get; set; }
        public int Ilosc { get; set; }
        public bool Dostepny { get; set; }
    }
}
