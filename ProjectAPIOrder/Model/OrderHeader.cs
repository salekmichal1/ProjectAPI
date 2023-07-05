using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPIOrder.Model
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public double OrderTotal { get; set; }
    }
}
