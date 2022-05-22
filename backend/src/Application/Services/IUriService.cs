using Application.Models;

namespace Application.Interfaces
{
    public interface IUriService
    {
        Uri GetCategoryUri(string categoryId);
        Uri GetItemUri(string itemId);

        Uri GetPageUri(PaginationFilter pagination = null);
    }
}
