using System.Net.Mime;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpGet(ApiRoutes.Categories.GetAll)]
    public IActionResult GetAll()
    {
        var categories = _unitOfWork.Category.GetAll();
        return Ok(categories);
    }

    [HttpGet(ApiRoutes.Categories.Get)]
    public IActionResult Get([FromRoute] int categoryId)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

        if (category == null) return NotFound();

        return Ok(category);
    }

    [HttpPost(ApiRoutes.Categories.Create)]
    [Authorize(Roles = "Admin")]
    public IActionResult Create([FromBody] CreateCategoryRequest categoryRequest)
    {
        var category = new Category {Name = categoryRequest.Name};

        _unitOfWork.Category.Add(category);
        _unitOfWork.Save();

        return Ok(category);
    }

    [HttpPut(ApiRoutes.Categories.Update)]
    [Authorize(Roles = "Admin")]
    public IActionResult Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

        if (category == null) return NotFound();

        category.Name = request.Name;

        _unitOfWork.Category.Update(category);
        _unitOfWork.Save();
        return Ok(category);
    }

    [HttpDelete(ApiRoutes.Categories.Delete)]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete([FromRoute] int categoryId)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

        if (category == null) return NotFound();

        _unitOfWork.Category.Remove(category);
        _unitOfWork.Save();

        return NoContent();
    }
}