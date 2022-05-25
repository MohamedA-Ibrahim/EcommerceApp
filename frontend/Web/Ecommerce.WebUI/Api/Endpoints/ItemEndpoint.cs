using Ecommerce.WebUI.Models;
using Ecommerce.WebUI.Models.Wrappers;
using System.Linq.Expressions;
using System.Net;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;

namespace Ecommerce.WebUI.Api
{
    public class ItemEndpoint : IItemEndpoint
    {
        private IApiHelper _apiHelper;

        public ItemEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PagedResponse<ItemResponse>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("items"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<PagedResponse<ItemResponse>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task CreateAsync(CreateItemRequest item)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("items", item))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<ItemResponse> UpdateAsync(UpdateItemRequest item,int id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync($"items/{id}", item);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ItemResponse>(); 
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"items/{id}");
            return response.StatusCode;
        }

        public async Task<ItemResponse> GetById(int? id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"items/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<ItemResponse>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }
}
