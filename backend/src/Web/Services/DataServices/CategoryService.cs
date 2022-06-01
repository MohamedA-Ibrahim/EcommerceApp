using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;
using Web.Helpers;
using Web.Services.DataServices.Interfaces;

namespace Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IUriService uriService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _uriService = uriService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CategoryResponse>> GetAllAsync(string categoryName, PaginationFilter paginationFilter)
        {
            List<Category> categories;

            if (categoryName != null)
                categories = await _unitOfWork.Category.GetAllAsync(x => x.Name.Contains(categoryName), paginationFilter);
            else
                categories = await _unitOfWork.Category.GetAllAsync(null, paginationFilter);

            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return new PagedResponse<CategoryResponse>(categoryResponse);
            }

            var totalRecords = await _unitOfWork.Category.CountAsync();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(categoryResponse, paginationFilter, totalRecords, _uriService);

            return paginationResponse;
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            return await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);
        }

        public async Task<Category> CreateAsync(CreateCategoryRequest categoryRequest)
        {
            var category = new Category { Name = categoryRequest.Name, Description = categoryRequest.Description, ImageUrl = categoryRequest.ImageUrl };

            await _unitOfWork.Category.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(int categoryId, UpdateCategoryRequest request, bool saveOldImage = false)
        {
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);

            if (category == null)
                return null;

            category.Name = request.Name;
            category.Description = request.Description;
            if (saveOldImage)
            {
                if (!string.IsNullOrWhiteSpace(request.ImageUrl))
                    category.ImageUrl = request.ImageUrl;
            }
            else
                category.ImageUrl = request.ImageUrl;

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return category;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);

            if (category == null)
                return false;

            _unitOfWork.Category.Remove(category);
            await _unitOfWork.SaveAsync();

            return true;
        }

    }
}
