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
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Items.GetAll)]
        public IActionResult GetAll()
        {
            IEnumerable<Item> items = _unitOfWork.Item.GetAll(includeProperties:"Category");

            return Ok(items);
        }

        /// <summary>
        /// Get an Item by Id
        /// </summary>
        /// <param name="itemId">The id of the item</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Items.Get)]
        public IActionResult Get([FromRoute] int itemId)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost(ApiRoutes.Items.Create)]
        public IActionResult Create([FromBody] CreateItemRequest request)
        {
            Item item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                ExpirationDate = request.ExpirationDate,
            };
            _unitOfWork.Item.Add(item);
            _unitOfWork.Save();

            return Ok(item);
        }

        [HttpPut(ApiRoutes.Items.Update)]
        public IActionResult Update([FromRoute] int itemId, [FromBody] UpdateItemRequest request)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == itemId);

            if (item == null)
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
        public IActionResult Delete([FromRoute] int itemId)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == itemId);

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
