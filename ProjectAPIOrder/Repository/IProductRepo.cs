using ProjectAPIOrder.Model.DTO;

namespace ProjectAPIOrder.Repository
{
    public interface IProductRepo
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
