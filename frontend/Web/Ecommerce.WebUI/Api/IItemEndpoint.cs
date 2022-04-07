using Ecommerce.WebUI.Models;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public interface IItemEndpoint
    {
        Task CreateAsync(Item item);
        Task<HttpStatusCode> DeleteAsync(int id);
        Task<IEnumerable<Item>> GetAll();
        Task<Item> GetById(int? id);
        Task<Item> UpdateAsync(Item item);
    }
}
