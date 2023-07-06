using Newtonsoft.Json;
using ProjectAPIOrder.Model.DTO;
using System.Text.Json.Serialization;

namespace ProjectAPIOrder.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductRepo(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        /// <summary>
        /// Pobieranie danych z innego projektu ProjectAPI, aby można było wyświetlić dane o zamówieniu zależne od produktów
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("ProjectAPI");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (resp.Accept)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDTO>();
        }
    }
}
