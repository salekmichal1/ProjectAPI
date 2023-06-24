using Microsoft.EntityFrameworkCore;
using ProjectAPI.Data;
using ProjectAPI.Model;
using static Azure.Core.HttpHeader;

namespace ProjectAPI.Repository
{
    /// <summary>
    /// Kalas imepletmecujaća interjest trzymający metody dla produktu
    /// </summary>
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Tworzenie produktu
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task CreateAsync(Product product)
        {
            _dbContext.Add(product);
        }

        /// <summary>
        /// Pobieranie wszytkisch produktów
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }
        
        /// <summary>
        /// Pobieranie pjedynczego produktu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetAsync(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Pobieranie pjedynczego po nazwie
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public async Task<Product> GetAsync(string productName)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(u => u.Name.ToLower() == productName.ToLower());
        }

        /// <summary>
        /// Usuwanie produktu
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        /// <summary>
        /// Zapisywanie produktu
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Aktualizacja produktu
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}
