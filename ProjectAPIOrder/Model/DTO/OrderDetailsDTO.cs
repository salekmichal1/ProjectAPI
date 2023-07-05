using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPIOrder.Model.DTO
{
    public class OrderDetailsDTO
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
    }
}
