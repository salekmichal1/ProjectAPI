using Microsoft.EntityFrameworkCore;
using ProjectAPI.Data;
using ProjectAPI.Model;
using static Azure.Core.HttpHeader;

namespace ProjectAPI.Repozytorium
{
    public class ProduktRepo : IProduktRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public ProduktRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Produkt produkt)
        {
            _dbContext.Add(produkt);
        }

        public async Task<ICollection<Produkt>> GetAllAsync()
        {
            return await _dbContext.Produkts.ToListAsync();
        }

        public async Task<Produkt> GetAsync(int id)
        {
            return await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Produkt> GetAsync(string produktNazwa)
        {
            return await _dbContext.Produkts.FirstOrDefaultAsync(u => u.Nazwa.ToLower() == produktNazwa.ToLower());
        }

        public async Task RemoveAsync(Produkt produkt)
        {
            _dbContext.Produkts.Remove(produkt);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Produkt produkt)
        {
            _dbContext.Produkts.Update(produkt);
        }
    }
}
