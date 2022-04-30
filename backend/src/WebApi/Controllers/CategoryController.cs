using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Contracts.V1.Responses;
using Application.Contracts.V1.Responses.Wrappers;

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


    [HttpGet(ApiRoutes.Categories.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationFilter)
    {
        var categories = await _unitOfWork.Category.GetAllAsync(paginationFilter);
        var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

        if (paginationFilter == null || paginationFilter.PageNumber <1 || paginationFilter.PageSize < 1)
        {
            return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
        }

        var nextPage = paginationFilter.PageNumber >= 1
            ? _uriService
            .GetAllCategoriesUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
            : null;

        var previousPage = paginationFilter.PageNumber -1 >= 1
    ? _uriService
    .GetAllCategoriesUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
    : null;

        var paginationResponse = new PagedResponse<CategoryResponse>
        {
            Data = categoryResponse,
            PageNumber = paginationFilter.PageNumber >=1 ? paginationFilter.PageNumber : (int?)null,
            PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
            NextPage = categoryResponse.Any() ? nextPage : null,
            PreviousPage = previousPage
        };
        return Ok(paginationResponse);
    }

    [HttpGet(ApiRoutes.Categories.Get)]
    public async Task<IActionResult> Get([FromRoute] int categoryId)
    {
        var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

        if (category == null) 
            return NotFound();

        return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
    }

    [HttpPost(ApiRoutes.Categories.Create)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest categoryRequest)
    {
        var category = new Category {Name = categoryRequest.Name};

        _unitOfWork.Category.Add(category);
        await _unitOfWork.SaveAsync();

        var locationUri = _uriService.GetCategoryUri(category.Id.ToString());
        return Created(locationUri, new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
    }

    [HttpPut(ApiRoutes.Categories.Update)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

        if (category == null) 
            return NotFound();

        category.Name = request.Name;

        _unitOfWork.Category.Update(category);
        await _unitOfWork.SaveAsync();

        return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
    }

    [HttpDelete(ApiRoutes.Categories.Delete)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int categoryId)
    {
        var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

        if (category == null) return NotFound();

        _unitOfWork.Category.Remove(category);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }
}