using Application.Common.Interfaces;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;
using Web.Helpers;
using Web.Services.DataServices.Interfaces;

namespace Web.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IUriService uriService, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _uriService = uriService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResponse<ItemResponse>> GetForSaleAsync(string query=null, PaginationFilter paginationFilter=null)
        {
            List<Item> items;

            if (query != null)
                items = await _unitOfWork.Item.GetAllIncludingAsync(x => !x.Sold && (x.Name.Contains(query)|| x.Category.Name.Contains(query)), paginationFilter, x => x.Category, u => u.Seller);
            else
                items = await _unitOfWork.Item.GetAllIncludingAsync(x => !x.Sold, paginationFilter, x => x.Category, u => u.Seller);

            var itemResponse = _mapper.Map<List<ItemResponse>>(items);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return new PagedResponse<ItemResponse>(itemResponse);
            }

            var totalRecords = await _unitOfWork.Item.CountAsync();
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(itemResponse, paginationFilter, totalRecords, _uriService);
            return paginationResponse;
        }

        public async Task<PagedResponse<ItemResponse>> GetForSaleByCategoryAsync(int categoryId, string itemName = null, PaginationFilter paginationFilter = null)
        {
            List<Item> items;

            if (itemName != null)
                items = await _unitOfWork.Item.GetAllIncludingAsync(x => x.CategoryId == categoryId && !x.Sold && x.Name.Contains(itemName), paginationFilter, x => x.Category, u => u.Seller);
            else
                items = await _unitOfWork.Item.GetAllIncludingAsync(x => x.CategoryId == categoryId && !x.Sold, paginationFilter, x => x.Category, u => u.Seller);

            var itemResponse = _mapper.Map<List<ItemResponse>>(items);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return new PagedResponse<ItemResponse>(itemResponse);
            }

            var totalRecords = await _unitOfWork.Item.CountAsync();
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(itemResponse, paginationFilter, totalRecords, _uriService);
            return paginationResponse;
        }

        public async Task<PagedResponse<ItemResponse>> GetPostedByUserAsync(PaginationFilter paginationFilter)
        {
            var items = await _unitOfWork.Item.GetAllIncludingAsync(x => x.SellerId == _currentUserService.UserId, paginationFilter, x => x.Category);
            var itemResponse = _mapper.Map<List<ItemResponse>>(items);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return new PagedResponse<ItemResponse>(itemResponse);
            }

            var totalRecords = await _unitOfWork.Item.CountAsync();
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(itemResponse, paginationFilter, totalRecords, _uriService);

            return paginationResponse;
        }

        public async Task<Item> GetAsync(int itemId)
        {
            return await _unitOfWork.Item.GetFirstOrDefaultAsync(itemId);
        }

        public async Task<Item> GetWithDetailsAsync(int itemId)
        {
            var iqItem = _unitOfWork.Item.DBSet.Where(x => x.Id == itemId)
                .Include(x => x.AttributeValues).ThenInclude(a => a.AttributeType)
                .Include(x => x.Category)
                .Include(x => x.Seller)
                .Include(x=>x.Orders).ThenInclude(b=> b.Buyer);
            return await iqItem.FirstOrDefaultAsync();
        }

        public async Task<Item> CreateAsync(CreateItemRequest request)
        {
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                ExpirationDate = request.ExpirationDate,
                SellerId = _currentUserService.UserId
            };

            await _unitOfWork.Item.AddAsync(item);
            await _unitOfWork.SaveAsync();

            return item;
        }

        public async Task<(bool success, string message)> DeleteAsync(int itemId)
        {
            var item = await _unitOfWork.Item.GetFirstOrDefaultIncludingAsync(itemId, x=> x.AttributeValues);

            if (item == null)
                return (false, "Not found");

            if (item.SellerId != _currentUserService.UserId)
                return (false, "You don't own this item");

            if (item.Sold)
                return (false, "You can't delete a sold item");

            var itemExistsInOrder = await _unitOfWork.Item.ItemExistsInOrder(itemId);
            if (itemExistsInOrder)
                return (false, "This item is placed in an order. it can't be deleted unless the order is rejected or cancelled");

            _unitOfWork.Item.Remove(item);
            await _unitOfWork.SaveAsync();

            return (true, "Delete Sucess");

        }

        public async Task<(Item item, string message)> UpdateAsync(int itemId, UpdateItemRequest request)
        {
            var item = await _unitOfWork.Item.GetFirstOrDefaultAsync(itemId);

            if (item == null)
                return (null, "Not found");

            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
            if (!userOwnsItem)
                return (null, "You don't own this item");

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.CategoryId = request.CategoryId;
            item.ExpirationDate = request.ExpirationDate;

            if (request.ImageUrl != null)
                item.ImageUrl = request.ImageUrl;

            _unitOfWork.Item.Update(item);
            await _unitOfWork.SaveAsync();

            return (item, "update success");
        }
    }
}
