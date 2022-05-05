using Application.Interfaces;
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
            var categories = await _unitOfWork.Category.GetAllAsync(null, paginationFilter);
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<CategoryResponse>(categoryResponse));
            }

            var totalRecords = await _unitOfWork.Category.CountAsync();

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(categoryResponse, paginationFilter, totalRecords, _uriService);
            return Ok(paginationResponse);
        }

        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] int categoryId)
        {
            var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

            if (category == null)
                return NotFound();

            return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
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

        [HttpPut(ApiRoutes.Orders.Update)]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _unitOfWork.Category.GetSingleAsync(categoryId);

            if (category == null)
                return NotFound();

            category.Name = request.Name;

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return Ok(new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category)));
        }
    }
}
