using Application.Contracts.V1.Responses.Wrappers;
using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationQuery paginationFilter, List<T> response)
        {
            var nextPage = paginationFilter.PageNumber >= 1
                ? uriService
                .GetAllCategoriesUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService
                .GetAllCategoriesUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;

            var paginationResponse = new PagedResponse<T>
            {
                Data = response,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };

            return paginationResponse;
        }
    }
}
