using ProjectAPI.Model;

namespace ProjectAPI.Data
{
    public static class ProduktySD
    {
        public static List<Produkt> produktyLista = new List<Produkt> {
            new Produkt{Id=1, Nazwa = "Kubek", Cena = 10.00, Ilosc = 10, Dostepny = true },
            new Produkt{Id=2, Nazwa = "Dlugopis", Cena = 20.00, Ilosc = 0, Dostepny = false }
         };
    }
}
