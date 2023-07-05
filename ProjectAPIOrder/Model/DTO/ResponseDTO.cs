namespace ProjectAPIOrder.Model.DTO
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool Accept { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
