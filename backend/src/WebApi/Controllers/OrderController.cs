using Application.Common.Interfaces;
using Application.Enums;
using Application.Interfaces;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;

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
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _emailService = emailService;
        }


        /// <summary>
        /// Get orders sold be the logged in user
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetSellerOrders)]
        public async Task<IActionResult> GetSellerOrders()
        {
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.SellerId == _currentUserService.UserId, null, x => x.Buyer, x => x.Seller, x => x.Item);
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

            if (userOwnsItem)
                return BadRequest(new { error = "You can't buy your own item!" });

            var sellerId = (await _unitOfWork.Item.GetFirstOrDefaultAsync(orderRequest.ItemId)).CreatedBy;

            var order = new Order
            {
                ItemId = orderRequest.ItemId,
                PhoneNumber = orderRequest.PhoneNumber,
                StreetAddress = orderRequest.StreetAddress,
                City = orderRequest.City,
                State = orderRequest.State,
                PostalCode = orderRequest.PostalCode,
                RecieverName = orderRequest.RecieverName,
                SellerId = sellerId,
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

            return Ok("Order status set to processing");
        }

        /// <summary>
        /// Update the status of order and payment to Approved and update item status to sold
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpPut(ApiRoutes.Orders.ConfirmPayment)]
        public async Task<IActionResult> ConfirmPayment([FromRoute] int orderId)
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

            return Ok("Order payment status updated successfully");
        }

        /// <summary>
        /// Update the status of order to Shipped, set the shipping date and send an email to the buyer
        /// </summary>
        /// <param name="orderId">The id of the order</param>

        [HttpPut(ApiRoutes.Orders.ShipOrder)]
        public async Task<IActionResult> ShipOrder([FromRoute] int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.ApplicationUser, x => x.Item);
            if (order == null)
                return NotFound();

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return BadRequest(new { error = "You are not the seller of this order" });

            if (order.OrderStatus == OrderStatus.StatusShipped)
                return BadRequest(new { error = "The order has already been shipped." });

            if (order.PaymentStatus != OrderStatus.PaymentStatusApproved)
                return BadRequest(new { error = "The order is still not paid!" });

            order.OrderStatus = OrderStatus.StatusShipped;
            order.ShippingDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();

            //await SendOrderShippedEmail(order.Buyer.Email, order.Item.Name);

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

            if (order.OrderStatus == OrderStatus.StatusShipped)
                return BadRequest(new { error = "The order has already been shipped. it can't be cancelled" });

            order.OrderStatus = OrderStatus.StatusCancelled;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Item.UpdateSoldStatus(order.ItemId, false);

            await _unitOfWork.SaveAsync();

            return Ok("Order cancelled successfully");
        }


        private async Task SendOrderShippedEmail(string email, string itemName)
        {
            await _emailService.SendEmailAsync(email, "Your order has been shipped", $"<p>This email is to confirm that your order {itemName} has been shipped!</p>");
        }

    }
}