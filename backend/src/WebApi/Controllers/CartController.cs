using System.Net.Mime;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CartController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;


    public CartController(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///  Get All Items
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Cart.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        var cart = await _unitOfWork.Cart.GetAllIncludingAsync(filter:x=> x.CreatedBy == _currentUserService.UserId, paginationFilter:null, includeProperties:x=> x.Item);
        var cartResponse = _mapper.Map<List<CartResponse>>(cart);

        return Ok(cartResponse);
    }



    [HttpPost(ApiRoutes.Cart.Add)]
    public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartRequest request)
    {
        var cartFromDb = await _unitOfWork.Cart.GetAllAsync(x=> x.CreatedBy == _currentUserService.UserId && x.ItemId == request.ItemId);
        if(cartFromDb.Count != 0)
        {
            return BadRequest(new { error = "Item is already added to cart!" });
        }

        await _unitOfWork.Cart.AddAsync(new Cart { ItemId = request.ItemId });
        await _unitOfWork.SaveAsync();

        return Ok();
    }

    [HttpDelete(ApiRoutes.Cart.Delete)]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] int cartId)
    {
        var cart = await _unitOfWork.Cart.GetSingleAsync(cartId);

        if (cart == null) 
            return NotFound();

        _unitOfWork.Cart.Remove(cart);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }

}