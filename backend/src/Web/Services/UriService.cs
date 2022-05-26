using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.WebUtilities;
using Web.Contracts.V1;

namespace Web.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(uri.ToString(), "pageNumber", pagination.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetCategoryUri(string categoryId)
        {
            return new Uri(_baseUri + ApiRoutes.Categories.Get.Replace("{categoryId}", categoryId));
        }

        public Uri GetItemUri(string itemId)
        {
            return new Uri(_baseUri + ApiRoutes.Items.Get.Replace("{itemId}", itemId));
        }


    }
}
