using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class CategoryController : Controller
{
    private readonly IMapper _mapper;
    private ICategoryService _categoryService;


    public CategoryController(IMapper mapper, ICategoryService categoryService)
    {
        _mapper = mapper;
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <param name="paginationFilter"></param>
    /// <returns></returns>

    [HttpGet(ApiRoutes.Categories.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] string? categoryName, [FromQuery] PaginationFilter paginationFilter)
    {
        var paginationResponse = await _categoryService.GetAllAsync(categoryName, paginationFilter);
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
        var category = await _categoryService.GetAsync(categoryId);

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
    [Authorize(Roles = "Admin",AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest categoryRequest)
    {     
        var category = await _categoryService.CreateAsync(categoryRequest);
        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    /// <summary>
    /// Update category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Categories.Update)]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var category = await _categoryService.UpdateAsync(categoryId, request);
        if(category == null)
            return NotFound();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Categories.Delete)]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Delete([FromRoute] int categoryId)
    {
       var deleted = await _categoryService.DeleteAsync(categoryId);
        if (!deleted.success)
            return BadRequest(deleted.message);

        return NoContent();
    }
}