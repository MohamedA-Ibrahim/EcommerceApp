using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet(ApiRoutes.Items.GetAll)]
        public IActionResult GetAll()
        {
            IEnumerable<Item> items = _unitOfWork.Item.GetAll();
            return Ok(items);
        }

        [HttpGet(ApiRoutes.Items.Get)]
        public IActionResult Get([FromRoute] int categoryId)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut(ApiRoutes.Items.Update)]
        public IActionResult Upsert([FromRoute] int id, [FromBody] UpdateItemRequest request)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x=> x.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.ImageUrl = request.ImageUrl;
            item.CategoryId = request.CategoryId;
            item.ExpirationDate = request.ExpirationDate;

            _unitOfWork.Item.Update(item);
            _unitOfWork.Save();
             return Ok(item);
        }

        [HttpDelete(ApiRoutes.Items.Delete)]
        public IActionResult Delete([FromRoute] int id)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _unitOfWork.Item.Remove(item);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
