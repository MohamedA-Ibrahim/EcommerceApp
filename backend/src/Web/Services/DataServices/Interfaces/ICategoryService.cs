using Application.Models;
using Domain.Entities;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;

namespace Web.Services.DataServices.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResponse<CategoryResponse>> GetAllAsync(string categoryName, PaginationFilter paginationFilter);
        Task<Category> GetAsync(int categoryId);
        Task<Category> CreateAsync(CreateCategoryRequest categoryRequest);
        Task<Category> UpdateAsync(int categoryId, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(int categoryId);
    }
}
