﻿using System.Net.Mime;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Models;
using AutoMapper;
using Application.Interfaces;
using Application.Contracts.V1.Responses;
using Application.Contracts.V1.Responses.Wrappers;
using Application.Helpers;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public ItemController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUriService uriService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _uriService = uriService;
        _mapper = mapper;
    }

    /// <summary>
    ///  Get All Items
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Items.GetAll)]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationFilter paginationFilter)
    {
        var items = await _unitOfWork.Item.GetAllIncludingAsync(null , paginationFilter, x=> x.Category);
        var itemResponse = _mapper.Map<List<ItemResponse>>(items);

        if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
        {
            return Ok(new PagedResponse<ItemResponse>(itemResponse));
        }

        var totalRecords = await _unitOfWork.Item.CountAsync();
        var paginationResponse = PaginationHelpers.CreatePaginatedResponse(itemResponse, paginationFilter, totalRecords, _uriService);

        return Ok(paginationResponse);
    }

    /// <summary>
    ///  Get items posted by user
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin,User")]
    [HttpGet(ApiRoutes.Items.GetUserItems)]
    public async Task<IActionResult> GetUserItemsAsync([FromQuery] PaginationFilter paginationFilter)
    {
        var items = await _unitOfWork.Item.GetAllIncludingAsync(x => x.CreatedBy == _currentUserService.UserId, paginationFilter, x => x.Category);
        var itemResponse = _mapper.Map<List<ItemResponse>>(items);

        if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
        {
            return Ok(new PagedResponse<ItemResponse>(itemResponse));
        }

        var totalRecords = await _unitOfWork.Item.CountAsync();
        var paginationResponse = PaginationHelpers.CreatePaginatedResponse(itemResponse, paginationFilter, totalRecords, _uriService);

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
        var item = await _unitOfWork.Item.GetSingleAsync(itemId);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    /// <summary>
    /// Create a new item for sale
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
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

        await _unitOfWork.Item.AddAsync(item);
        await _unitOfWork.SaveAsync();


        var locationUri = _uriService.GetItemUri(item.Id.ToString());
        return Created(locationUri, new Response<ItemResponse>(_mapper.Map<ItemResponse>(item)));
    }

    /// <summary>
    /// Update an item
    /// </summary>
    /// <param name="itemId">The id of the item to update</param>
    /// <param name="request">The updated info</param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Items.Update)]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Update([FromRoute] int itemId, [FromBody] UpdateItemRequest request)
    {
        var item = await _unitOfWork.Item.GetSingleAsync(itemId);

        if (item == null) 
            return NotFound();

        var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(itemId, _currentUserService.UserId);
        if (!userOwnsItem) 
            return BadRequest(new {error = "You don't own this item"});

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.CategoryId = request.CategoryId;
        item.ExpirationDate = request.ExpirationDate;

        if (request.ImageUrl != null)
            item.ImageUrl = request.ImageUrl;

        _unitOfWork.Item.Update(item);
        await _unitOfWork.SaveAsync();

        return Ok(new Response<ItemResponse>(_mapper.Map<ItemResponse>(item)));
    }

    /// <summary>
    /// Delete an item
    /// </summary>
    /// <param name="itemId">The id of the item to delete</param>
    /// <returns></returns>

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