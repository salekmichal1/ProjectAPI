using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Model.DTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }
    }
}
