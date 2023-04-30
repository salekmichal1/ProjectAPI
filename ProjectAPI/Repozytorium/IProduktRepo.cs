using ProjectAPI.Model;
using static Azure.Core.HttpHeader;

namespace ProjectAPI.Repozytorium
{
    public interface IProduktRepo
    {
        Task<ICollection<Produkt>> GetAllAsync();
        Task<Produkt> GetAsync(int id);
        Task<Produkt> GetAsync(string produktNazwa);
        Task CreateAsync(Produkt produkt);
        Task UpdateAsync(Produkt produkt);
        Task RemoveAsync(Produkt produkt);
        Task SaveAsync();
    }
}
