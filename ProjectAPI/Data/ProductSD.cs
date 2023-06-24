using ProjectAPI.Model;

namespace ProjectAPI.Data
{
    public static class ProductSD
    {
        public static List<Product> productsList = new List<Product> {
            new Product{Id=1, Name = "Kubek", Price = 10.00, Quantity = 10, Available = true },
            new Product{Id=2, Name = "Dlugopis", Price = 20.00, Quantity = 0, Available = false }
         };
    }
}
