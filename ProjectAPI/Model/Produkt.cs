namespace ProjectAPI.Model
{
    public class Produkt
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public double Cena { get; set; }
        public int Ilosc { get; set; }
        public bool Dostepny { get; set; }
        public DateTime? Utworzony { get; set; }
        public DateTime? Zaktualizowany { get; set; }
    }
}
