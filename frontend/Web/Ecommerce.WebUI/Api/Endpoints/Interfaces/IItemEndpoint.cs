using Ecommerce.WebUI.Models;
using Ecommerce.WebUI.Models.Wrappers;
using System.Linq.Expressions;
using System.Net;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;

namespace Ecommerce.WebUI.Api
{
    public interface IItemEndpoint
    {
        Task CreateAsync(CreateItemRequest item);
        Task<HttpStatusCode> DeleteAsync(int id);
        Task<PagedResponse<ItemResponse>> GetAll();
        Task<ItemResponse> GetById(int? id);
        Task<ItemResponse> UpdateAsync(UpdateItemRequest item, int id);
    }
}
