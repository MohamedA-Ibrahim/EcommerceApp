using Ecommerce.WebUI.Models;
using System.Linq.Expressions;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public class ItemEndpoint : IItemEndpoint
    {
        private IApiHelper _apiHelper;

        public ItemEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("items"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<Item>>();
                    return result.Select(x => new Item
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Description = x.Description,
                        ExpirationDate = x.ExpirationDate,
                        Category = x.Category,
                        ImageUrl = _apiHelper.ServerUrl + x.ImageUrl,
                    });
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task CreateAsync(Item item)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("items", item))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync($"items/{item.Id}", item);
            response.EnsureSuccessStatusCode();

            item = await response.Content.ReadAsAsync<Item>();
            return item;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"items/{id}");
            return response.StatusCode;
        }

        public async Task<Item> GetById(int? id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"items/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Item>();
                    result.ImageUrl = _apiHelper.ServerUrl + result.ImageUrl;

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
