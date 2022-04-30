using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.WebUtilities;
using Application.Contracts.V1;

namespace WebApi.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetCategoryUri(string categoryId)
        {
            return new Uri(_baseUri + ApiRoutes.Categories.Get.Replace("{categoryId}", categoryId));
        }

        public Uri GetAllCategoriesUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if(pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(uri.ToString(), "pageNumber", pagination.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            
            return new Uri(modifiedUri);
        }
    }
}
