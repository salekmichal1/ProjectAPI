using ProjectAPI.Model;
using static Azure.Core.HttpHeader;

namespace ProjectAPI.Repository
{
    public interface IProductRepo
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> GetAsync(string productName);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task RemoveAsync(Product product);
        Task SaveAsync();
    }
}
