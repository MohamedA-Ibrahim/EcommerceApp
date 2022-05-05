﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Contracts.V1.Responses;
using Application.Contracts.V1.Responses.Wrappers;
using Application.Helpers;
using Application.Enums;
using Application.Utils;
using Application.Common.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ICurrentUserService _currentUserService;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IUriService uriService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uriService = uriService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Get user orders
        /// </summary>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Orders.GetUserOrders)]
        public async Task<IActionResult> GetUserOrders([FromQuery] PaginationFilter paginationFilter)
        {        
            //TODO: Implement
            return Ok("Not implemented yet");
        }

        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] int categoryId)
        {
            //TODO: Implement
            return Ok("Not implemented yet");
        }

        /// <summary>
        /// Create an order when purchasing
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>

        [HttpPost(ApiRoutes.Orders.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
        {
            var order = new Order 
            {
                OrderStatus = OrderStatus.StatusPending,
                PaymentStatus = OrderStatus.PaymentStatusPending,
                OrderDate = DateUtil.GetCurrentDate(),
                BuyerId = _currentUserService.UserId,
                PhoneNumber = orderRequest.PhoneNumber,
                StreetAddress = orderRequest.StreetAddress,
                City = orderRequest.City,
                State = orderRequest.State,
                PostalCode  = orderRequest.PostalCode,
                RecieverName = orderRequest.UserName,
            };

            OrderDetail orderDetail = new OrderDetail
            {
                Order = order,
                ItemId = orderRequest.ItemId,
                OrderId = order.Id,
                OrderTotal = orderRequest.OrderTotal,
                Count = 1
            };

            await _unitOfWork.OrderDetail.AddAsync(orderDetail);
            await _unitOfWork.SaveAsync();

            return Ok(order);
        }
    }
}
