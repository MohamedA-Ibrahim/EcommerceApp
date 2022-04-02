using Ecommerce.WebUI.Models;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public interface ICategoryEndpoint
    {
        Task Create(Category cat);
        Task<HttpStatusCode> DeleteAsync(int id);
        Task<List<Category>> GetAll();
        Task<Category> GetById(int? id);
        Task<Category> UpdateAsync(Category category);
    }
}