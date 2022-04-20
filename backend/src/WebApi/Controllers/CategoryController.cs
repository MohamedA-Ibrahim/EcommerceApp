using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return Ok(categories);
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        public IActionResult Get([FromRoute] int categoryId)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost(ApiRoutes.Categories.Create)]
        public IActionResult Create([FromBody] CreateCategoryRequest categoryRequest)
        {
            Category category = new Category{Name = categoryRequest.Name};

            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            return Ok(category);
        }

        [HttpPut(ApiRoutes.Categories.Update)]
        public IActionResult Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            //var userOwnsItem = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            //if (!userOwnsItem)
            //{
            //    return BadRequest(new { error = "You don't own this item" });
            //}

            var category = _unitOfWork.Category.GetFirstOrDefault(x=> x.Id == categoryId);

            if(category == null)
            {
                return NotFound();
            }

            category.Name = request.Name;

            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
             return Ok(category);
        }

        [HttpDelete(ApiRoutes.Categories.Delete)]
        public IActionResult Delete([FromRoute] int categoryId)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
