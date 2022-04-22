using Application.Common.Interfaces;
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
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ItemController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
        public IActionResult Create([FromBody] CreateItemRequest request, [FromForm] IFormFile? file)
        {
            Item item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Image = request.Image,
                CategoryId = request.CategoryId,
                ExpirationDate = request.ExpirationDate,
            };

            _unitOfWork.Item.Add(item);
            _unitOfWork.Save();

            return Ok(item);
        }

        [HttpPut(ApiRoutes.Items.Update)]
        public async Task<IActionResult> Update([FromRoute] int itemId, [FromBody] UpdateItemRequest request)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
            if (!userOwnsItem)
            {
                return BadRequest(new {error ="You don't own this item"});
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Image = request.Image;
            item.CategoryId = request.CategoryId;
            item.ExpirationDate = request.ExpirationDate;

            _unitOfWork.Item.Update(item);
            _unitOfWork.Save();
            return Ok(item);
        }

        [HttpDelete(ApiRoutes.Items.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int itemId)
        {
            var item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
            if (!userOwnsItem)
            {
                return BadRequest(new { error = "You don't own this item" });
            }

            _unitOfWork.Item.Remove(item);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
