using Domain.Entities;
using Ecommerce.WebUI.Models;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public class CategoryEndpoint : ICategoryEndpoint
    {
        private IApiHelper _apiHelper;

        public CategoryEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("categories"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<Category>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Create(Category cat)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("categories", cat))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync($"categories/{category.Id}", category);
            response.EnsureSuccessStatusCode();

            category = await response.Content.ReadAsAsync<Category>();
            return category;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"categories/{id}");
            return response.StatusCode;
        }

        public async Task<Category> GetById(int? id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"categories/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Category>();
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
