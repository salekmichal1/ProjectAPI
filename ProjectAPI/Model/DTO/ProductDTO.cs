namespace ProjectAPI.Model.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }
        public DateTime? Created { get; set; }

    }
}
