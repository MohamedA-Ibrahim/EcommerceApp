using Application.Common.Interfaces;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Contracts.V1.Responses.Wrappers;
using Web.Helpers;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IItemService _itemService;

    public ItemController(IMapper mapper, IItemService itemService)
    {
        _mapper = mapper;
        _itemService = itemService;
    }

    /// <summary>
    /// Get items for sale with category and seller details
    /// </summary>
    /// <param name="itemName">Search items by item name (optional)</param>
    [HttpGet(ApiRoutes.Items.GetForSale)]
    public async Task<IActionResult> GetForSaleAsync([FromQuery] string? itemName, [FromQuery] PaginationFilter paginationFilter)
    {      
        var paginationResponse = await _itemService.GetForSaleAsync(itemName, paginationFilter);

        return Ok(paginationResponse);
    }

    /// <summary>
    /// Get items for sale with by category
    /// </summary>
    /// <param name="itemName">Search items by item name (optional)</param>
    [HttpGet(ApiRoutes.Items.GetForSaleByCategory)]
    public async Task<IActionResult> GetForSaleByCategoryAsync([FromQuery] int categoryId, [FromQuery] string? itemName, [FromQuery] PaginationFilter paginationFilter)
    {
        var paginationResponse = await _itemService.GetForSaleByCategoryAsync(categoryId, itemName, paginationFilter);

        return Ok(paginationResponse);
    }

    /// <summary>
    ///  Get items posted by user
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    [HttpGet(ApiRoutes.Items.GetUserItems)]
    public async Task<IActionResult> GetPostedByUserAsync([FromQuery] PaginationFilter paginationFilter)
    {
        var paginationResponse = await _itemService.GetPostedByUserAsync(paginationFilter);

        return Ok(paginationResponse);
    }


    /// <summary>
    /// Get an Item by Id
    /// </summary>
    /// <param name="itemId">The id of the item</param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Items.Get)]
    public async Task<IActionResult> GetById([FromRoute] int itemId)
    {
        var item = await _itemService.GetAsync(itemId);

        if (item == null)
            return NotFound();

        return Ok(_mapper.Map<ItemResponse>(item));
    }

    /// <summary>
    /// Get an Item by Id with its details(category, attribute values, seller and orders for this item)
    /// </summary>
    /// <param name="itemId">The id of the item</param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Items.GetWithOrders)]
    public async Task<IActionResult> GetByIdWithOrders([FromRoute] int itemId)
    {
        var item = await _itemService.GetWithDetailsAsync(itemId);

        if (item == null)
            return NotFound();

        return Ok(_mapper.Map<ItemResponse>(item));
    }


    /// <summary>
    /// Create a new item for sale
    /// </summary>
    /// <param name="request"></param>
    [HttpPost(ApiRoutes.Items.Create)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request)
    {
        var item = await _itemService.CreateAsync(request);
        return Ok(_mapper.Map<ItemResponse>(item));
    }

    /// <summary>
    /// Update an item
    /// </summary>
    /// <param name="itemId">The id of the item to update</param>
    /// <param name="request">The updated info</param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Items.Update)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update([FromRoute] int itemId, [FromBody] UpdateItemRequest request)
    {
        var itemStatus = await _itemService.UpdateAsync(itemId, request);
        if(itemStatus.item == null)
            return BadRequest(new {error = itemStatus.message});

        return Ok(_mapper.Map<ItemResponse>(itemStatus.item));
    }

    /// <summary>
    /// Delete an item
    /// </summary>
    /// <param name="itemId">The id of the item to delete</param>
    /// <returns></returns>

    [HttpDelete(ApiRoutes.Items.Delete)]
    [Authorize(Roles = "Admin,User", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Delete([FromRoute] int itemId)
    {
        var status = await _itemService.DeleteAsync(itemId);
        
        if(!status.success)
            return BadRequest(new {error = status.message});

        return NoContent();
    }
}