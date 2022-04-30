using System.Net.Mime;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public ItemController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Get All Items
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Items.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        var items = await _unitOfWork.Item.GetAllIncludingAsync(null, x=> x.Category);

        return Ok(items);
    }

    /// <summary>
    ///     Get an Item by Id
    /// </summary>
    /// <param name="itemId">The id of the item</param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Items.Get)]
    public async Task<IActionResult> GetById([FromRoute] int itemId)
    {
        var item = await _unitOfWork.Item.GetSingleAsync(itemId);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost(ApiRoutes.Items.Create)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request)
    {
        var item = new Item
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            ExpirationDate = request.ExpirationDate
        };

        _unitOfWork.Item.Add(item);
        await _unitOfWork.SaveAsync();

        return Ok(item);
    }

    [HttpPut(ApiRoutes.Items.Update)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Update([FromRoute] int itemId, [FromBody] UpdateItemRequest request)
    {
        var item = await _unitOfWork.Item.GetSingleAsync(itemId);

        if (item == null) return NotFound();

        var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
        if (!userOwnsItem) return BadRequest(new {error = "You don't own this item"});

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.ImageUrl = request.ImageUrl;
        item.CategoryId = request.CategoryId;
        item.ExpirationDate = request.ExpirationDate;

        _unitOfWork.Item.Update(item);
        await _unitOfWork.SaveAsync();
        return Ok(item);
    }

    [HttpDelete(ApiRoutes.Items.Delete)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Delete([FromRoute] int itemId)
    {
        var item = await _unitOfWork.Item.GetSingleAsync(itemId);

        if (item == null) return NotFound();

        var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
        if (!userOwnsItem) return BadRequest(new {error = "You don't own this item"});

        _unitOfWork.Item.Remove(item);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }
}