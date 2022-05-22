using Application.Interfaces;
using Application.Models;
using WebApi.Contracts.V1.Responses.Wrappers;

namespace WebApi.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(List<T> data, PaginationFilter paginationFilter, int totalRecords, IUriService uriService)
        {
            //Create a pagination response
            var response = new PagedResponse<T>
            {
                Data = data,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null
            };

            //Get total pages
            var totalPages = ((double)totalRecords / (double)paginationFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            //Get next and previous page urls
            var nextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < roundedTotalPages
                ? uriService
                .GetPageUri(new PaginationFilter(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1 && paginationFilter.PageNumber <= roundedTotalPages
                ? uriService
                .GetPageUri(new PaginationFilter(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;

            response.NextPage = data.Any() ? nextPage : null;
            response.PreviousPage = previousPage;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, paginationFilter.PageSize)).ToString();
            response.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, paginationFilter.PageSize)).ToString();
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;


            return response;
        }
    }
}
