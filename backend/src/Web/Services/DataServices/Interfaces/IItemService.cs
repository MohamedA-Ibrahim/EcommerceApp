using Application.Models;
using Domain.Entities;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;

namespace Web.Services.DataServices.Interfaces
{
    public interface IItemService
    {
        Task<Item> CreateAsync(CreateItemRequest request);
        Task<(bool success, string message)> DeleteAsync(int itemId);
        Task<Item> GetAsync(int itemId);
        Task<PagedResponse<ItemResponse>> GetForSaleAsync(string itemName = null, PaginationFilter paginationFilter = null);
        Task<PagedResponse<ItemResponse>> GetPostedByUserAsync(PaginationFilter paginationFilter);
        Task<(Item item, string message)> UpdateAsync(int itemId, UpdateItemRequest request);
    }
}
