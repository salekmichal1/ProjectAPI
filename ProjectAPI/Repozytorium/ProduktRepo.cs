using Microsoft.EntityFrameworkCore;
using ProjectAPI.Data;
using ProjectAPI.Model;
using static Azure.Core.HttpHeader;

namespace ProjectAPI.Repozytorium
{
    /// <summary>
    /// Kalas imepletmecujaća interjest trzymający metody dla produktu
    /// </summary>
    public class ProduktRepo : IProduktRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public ProduktRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Tworzenie produktu
        /// </summary>
        /// <param name="produkt"></param>
        /// <returns></returns>
        public async Task CreateAsync(Produkt produkt)
        {
            _dbContext.Add(produkt);
        }

        /// <summary>
        /// Pobieranie wszytkisch produktów
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Produkt>> GetAllAsync()
        {
            return await _dbContext.Produkts.ToListAsync();
        }
        
        /// <summary>
        /// Pobieranie pjedynczego produktu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Produkt> GetAsync(int id)
        {
            return await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Pobieranie pjedynczego po nazwie
        /// </summary>
        /// <param name="produktNazwa"></param>
        /// <returns></returns>
        public async Task<Produkt> GetAsync(string produktNazwa)
        {
            return await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Nazwa.ToLower() == produktNazwa.ToLower());
        }

        /// <summary>
        /// Usuwanie produktu
        /// </summary>
        /// <param name="produkt"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Produkt produkt)
        {
            _dbContext.Produkts.Remove(produkt);
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
        /// <param name="produkt"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Produkt produkt)
        {
            _dbContext.Produkts.Update(produkt);
        }
    }
}
