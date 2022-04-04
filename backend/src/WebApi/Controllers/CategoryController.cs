using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;

        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }


        [HttpGet(ApiRoutes.Categories.GetAll)]
        public IActionResult GetAll()
        {
            IEnumerable<Category> categories = _db.GetAll();
            return Ok(categories);
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        public IActionResult Get([FromRoute] int categoryId)
        {
            var category = _db.GetFirstOrDefault(x => x.Id == categoryId);

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

            _db.Add(category);
            _db.SaveChanges();

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

            var category = _db.GetFirstOrDefault(x=> x.Id == categoryId);

            if(category == null)
            {
                return NotFound();
            }

            category.Name = request.Name;

            _db.Update(category);
            _db.SaveChanges();
             return Ok(category);
        }

        [HttpDelete(ApiRoutes.Categories.Delete)]
        public IActionResult Delete([FromRoute] int categoryId)
        {
            var category = _db.GetFirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            _db.Remove(category);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
