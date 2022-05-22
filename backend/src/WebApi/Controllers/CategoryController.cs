using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;
using WebApi.Contracts.V1.Responses.Wrappers;
using WebApi.Helpers;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, IUriService uriService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uriService = uriService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <returns></returns>

    [HttpGet(ApiRoutes.Categories.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
    {
        var categories = await _unitOfWork.Category.GetAllAsync(null, paginationFilter);
        var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

        if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
        {
            return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
        }

        var totalRecords = await _unitOfWork.Category.CountAsync();

        var paginationResponse = PaginationHelpers.CreatePaginatedResponse(categoryResponse, paginationFilter, totalRecords, _uriService);
        return Ok(paginationResponse);
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Categories.Get)]
    public async Task<IActionResult> Get([FromRoute] int categoryId)
    {
        var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);

        if (category == null)
            return NotFound();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    /// <summary>
    /// Create a category
    /// </summary>
    /// <param name="categoryRequest"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Categories.Create)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest categoryRequest)
    {
        var category = new Category { Name = categoryRequest.Name, Description = categoryRequest.Description, ImageUrl = categoryRequest.ImageUrl };

        await _unitOfWork.Category.AddAsync(category);
        await _unitOfWork.SaveAsync();

        var locationUri = _uriService.GetCategoryUri(category.Id.ToString());
        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    /// <summary>
    /// Update category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Categories.Update)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);

        if (category == null)
            return NotFound();

        category.Name = request.Name;
        category.Description = request.Description;
        category.ImageUrl = request.ImageUrl;

        _unitOfWork.Category.Update(category);
        await _unitOfWork.SaveAsync();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Categories.Delete)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int categoryId)
    {
        var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(categoryId);

        if (category == null) return NotFound();

        _unitOfWork.Category.Remove(category);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }
}