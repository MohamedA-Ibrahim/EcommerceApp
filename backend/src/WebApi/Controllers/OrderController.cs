using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Application.Contracts.V1;
using Application.Contracts.V1.Requests;
using Application.Contracts.V1.Responses;
using Application.Enums;
using Application.Utils;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// Get orders sold be the logged in user
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetSellerOrders)]
        public async Task<IActionResult> GetSellerOrders()
        {
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.SellerId == _currentUserService.UserId, null, x=> x.Buyer, x=> x.Seller, x=> x.Item);
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            return Ok(orderResponse);
        }

        /// <summary>
        /// get orders bought by the logged in user
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetBuyerOrders)]
        public async Task<IActionResult> GetBuyerOrders()
        {
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.BuyerId == _currentUserService.UserId, null, x => x.Buyer, x => x.Seller, x => x.Item);
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            return Ok(orderResponse);
        }


        /// <summary>
        /// Get an order info by Id
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpGet(ApiRoutes.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.Buyer, x => x.Seller, x => x.Item);
            var orderResponse = _mapper.Map<OrderResponse>(order);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Create an order when a user buys an item
        /// </summary>
        /// <param name="orderRequest">The order info</param>
        [HttpPost(ApiRoutes.Orders.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
        {
            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(orderRequest.ItemId, _currentUserService.UserId);

            if(userOwnsItem)
                return BadRequest(new { error = "You can't buy your own item!" });

            var order = new Order 
            {
                ItemId = orderRequest.ItemId,
                SellerId = orderRequest.SellerId,
                PhoneNumber = orderRequest.PhoneNumber,
                StreetAddress = orderRequest.StreetAddress,
                City = orderRequest.City,
                State = orderRequest.State,
                PostalCode  = orderRequest.PostalCode,
                RecieverName = orderRequest.RecieverName,
                BuyerId = _currentUserService.UserId,
                OrderDate = DateUtil.GetCurrentDate(),
                OrderStatus = OrderStatus.StatusPending,
                PaymentStatus = OrderStatus.PaymentStatusPending
            };


            await _unitOfWork.Order.AddAsync(order);
            _unitOfWork.Item.UpdateSoldStatus(orderRequest.ItemId, true);

            await _unitOfWork.SaveAsync();

            var orderResponse = _mapper.Map<OrderResponse>(order);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Update the order status to Processing, payment status to Pending
        /// </summary>
        /// <param name="orderId">The id of the order to update</param>
        [HttpPut(ApiRoutes.Orders.StartProcessing)]
        public async Task<IActionResult> StartProcessing([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return NotFound();

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return BadRequest(new { error = "You are not the seller of this order" });

            order.OrderStatus = OrderStatus.StatusInProcess;
            order.PaymentStatus = OrderStatus.PaymentStatusPending;

            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();

            return Ok("Order status updated successfully");
        }

        /// <summary>
        /// Update the status of order and payment to Approved and update item status to sold
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpPut(ApiRoutes.Orders.UpdatePayment)]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return NotFound();

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return BadRequest(new { error = "You are not the seller of this order" });

            order.OrderStatus = OrderStatus.StatusApproved;
            order.PaymentStatus = OrderStatus.PaymentStatusApproved;
            order.PaymentDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();

            return Ok("Order status updated successfully");
        }

        /// <summary>
        /// Update the status of order to Shipped and set the shipping date to current date
        /// </summary>
        /// <param name="orderId">The id of the order</param>

        [HttpPut(ApiRoutes.Orders.ShipOrder)]
        public async Task<IActionResult> ShipOrder([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if(order == null)
                return NotFound();

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return BadRequest(new { error = "You are not the seller of this order" });

            order.OrderStatus = OrderStatus.StatusShipped;
            order.ShippingDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();

            return Ok("Order shipped status updated successfully");
        }

        /// <summary>
        /// Cancel an order and set item sold status to false
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpPut(ApiRoutes.Orders.CancelOrder)]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return NotFound();

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerOrBuyerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return BadRequest(new { error = "You are not a seller or a buyer of this order" });

            if(order.OrderStatus == OrderStatus.StatusShipped)
                return BadRequest(new { error = "The order has already been shipped. it can't be cancelled" });

            order.OrderStatus = OrderStatus.StatusCancelled;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Item.UpdateSoldStatus(order.ItemId, false);

            await _unitOfWork.SaveAsync();

            return Ok("Order cancelled successfully");
        }

    }
}