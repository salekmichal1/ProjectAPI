namespace ProjectAPIOrder.Model.DTO
{
    public class OrderDTO
    {
        public OrderHeaderDTO OrderHeader { get; set; }
        public IEnumerable<OrderDetailsDTO>? OrderDetails { get; set; }
    }
}
