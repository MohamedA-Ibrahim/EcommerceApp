using Application.Common.Interfaces;
using AutoMapper;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, ICurrentUserService currentUserService, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }


        /// <summary>
        /// Get orders sold be the logged in user
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetSellerOrders)]
        public async Task<IActionResult> GetSellerOrders()
        {
            var orders = await _orderService.GetSellerOrdersAsync();
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            return Ok(orderResponse);
        }

        /// <summary>
        /// get orders bought by the logged in user
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetBuyerOrders)]
        public async Task<IActionResult> GetBuyerOrders()
        {
            var orders = await _orderService.GetBuyerOrdersAsync();
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
            var order = await _orderService.GetAsync(orderId);
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
            var orderStatus = await _orderService.CreateOrderAsync(orderRequest);
            if(orderStatus.order == null)
                return BadRequest(orderStatus.message);

            var orderResponse = _mapper.Map<OrderResponse>(orderStatus.order);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Update the order status to Processing, payment status to Pending
        /// </summary>
        /// <param name="orderId">The id of the order to update</param>
        [HttpPut(ApiRoutes.Orders.StartProcessing)]
        public async Task<IActionResult> StartProcessing([FromRoute] int orderId)
        {
           var orderStatus = await _orderService.StartProcessingAsync(orderId);
            if(!orderStatus.success)
                return BadRequest(new {error = orderStatus.message});

            return Ok(orderStatus.message);
        }

        /// <summary>
        /// Update the status of order and payment to Approved and update item status to sold
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpPut(ApiRoutes.Orders.ConfirmPayment)]
        public async Task<IActionResult> ConfirmPayment([FromRoute] int orderId)
        {
            var orderStatus = await _orderService.ConfirmPaymentAsync(orderId);
            if (!orderStatus.success)
                return BadRequest(new { error = orderStatus.message });

            return Ok(orderStatus.message);
        }

        /// <summary>
        /// Update the status of order to Shipped, set the shipping date and send an email to the buyer
        /// </summary>
        /// <param name="orderId">The id of the order</param>

        [HttpPut(ApiRoutes.Orders.ShipOrder)]
        public async Task<IActionResult> ShipOrder([FromRoute] int orderId)
        {
            var orderStatus = await _orderService.ShipOrderAsync(orderId);
            if (!orderStatus.success)
                return BadRequest(new { error = orderStatus.message });

            return Ok(orderStatus.message);
        }

        /// <summary>
        /// Cancel an order and set item sold status to false
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        [HttpPut(ApiRoutes.Orders.CancelOrder)]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            var orderStatus = await _orderService.CancelOrderAsync(orderId);
            if (!orderStatus.success)
                return BadRequest(new { error = orderStatus.message });

            return Ok(orderStatus.message);
        }
    }
}