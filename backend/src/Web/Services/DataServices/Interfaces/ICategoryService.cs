using Application.Models;
using Domain.Entities;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;

namespace Web.Services.DataServices.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResponse<CategoryResponse>> GetAll(string categoryName, PaginationFilter paginationFilter);
        Task<Category> Get(int categoryId);
        Task<Category> Create(CreateCategoryRequest categoryRequest);
        Task<Category> Update(int categoryId, UpdateCategoryRequest request);
        Task<bool> Delete(int categoryId);
    }
}
