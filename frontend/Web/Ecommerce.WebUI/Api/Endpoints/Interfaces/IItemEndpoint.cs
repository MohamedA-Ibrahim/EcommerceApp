using Ecommerce.WebUI.Models;
using Ecommerce.WebUI.Models.Wrappers;
using System.Linq.Expressions;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public interface IItemEndpoint
    {
        Task CreateAsync(Item item);
        Task<HttpStatusCode> DeleteAsync(int id);
        Task<PagedResponse<Item>> GetAll();
        Task<Item> GetById(int? id);
        Task<Item> UpdateAsync(Item item);
    }
}
