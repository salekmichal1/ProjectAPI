using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPIOrder.Model.DTO
{
    public class OrderHeaderDTO
    {
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }

        public double OrderTotal { get; set; }
    }
}
