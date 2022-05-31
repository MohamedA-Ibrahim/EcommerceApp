using Application.Common.Interfaces;
using Application.Enums;
using Application.Interfaces;
using Application.Utils;
using Domain.Entities;
using Infrastructure.Repository;
using Web.Contracts.V1.Requests;
using Web.Services.DataServices.Interfaces;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _emailService = emailService;
        }


        public async Task<List<Order>> GetSellerOrdersAsync()
        {
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.SellerId == _currentUserService.UserId, null, x => x.ApplicationUser, x => x.Seller, x => x.Item);

            return orders;
        }


        public async Task<List<Order>> GetBuyerOrdersAsync()
        {
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.CreatedBy == _currentUserService.UserId, null, x => x.ApplicationUser, x => x.Seller, x => x.Item);

            return orders;
        } 

        public async Task<Order> GetAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.ApplicationUser, x => x.Seller, x => x.Item);
            return order;
        }

        public async Task<(Order order, string message)> CreateOrderAsync(CreateOrderRequest orderRequest)
        {
            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(orderRequest.ItemId, _currentUserService.UserId);

            if (userOwnsItem)
                return (null, "You can't buy your own item!");

            var sellerId = (await _unitOfWork.Item.GetFirstOrDefaultAsync(orderRequest.ItemId)).CreatedBy;

            var order = new Order
            {
                ItemId = orderRequest.ItemId,
                PhoneNumber = orderRequest.PhoneNumber,
                StreetAddress = orderRequest.StreetAddress,
                City = orderRequest.City,
                RecieverName = orderRequest.RecieverName,
                SellerId = sellerId,
                OrderDate = DateUtil.GetCurrentDate(),
                OrderStatus = OrderStatus.StatusPending,
                PaymentStatus = OrderStatus.PaymentStatusPending
            };


            await _unitOfWork.Order.AddAsync(order);
            _unitOfWork.Item.UpdateSoldStatus(orderRequest.ItemId, true);

            await _unitOfWork.SaveAsync();

            return (order, "success");
        }

        public async Task<(bool success, string message)> StartProcessingAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not the seller of this order");

            order.OrderStatus = OrderStatus.StatusInProcess;
            order.PaymentStatus = OrderStatus.PaymentStatusPending;

            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();

            return (true, "Order status set to processing");
        }


        public async Task<(bool success, string message)> ConfirmPaymentAsync( int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not the seller of this order");

            order.OrderStatus = OrderStatus.StatusApproved;
            order.PaymentStatus = OrderStatus.PaymentStatusApproved;
            order.PaymentDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();

            return (true, "Order payment status updated successfully");
        }

        public async Task<(bool success, string message)> ShipOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.ApplicationUser, x => x.Item);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not the seller of this order");

            if (order.OrderStatus == OrderStatus.StatusShipped)
                return (false, "The order has already been shipped.");

            if (order.PaymentStatus != OrderStatus.PaymentStatusApproved)
                return (false, "The order is still not paid!");

            order.OrderStatus = OrderStatus.StatusShipped;
            order.ShippingDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();

            //await SendOrderShippedEmail(order.Buyer.Email, order.Item.Name);


            return (true, "Order shipped status updated successfully");
        }

        public async Task<(bool success, string message)> CancelOrderAsync( int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerOrBuyerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not a seller or a buyer of this order");

            if (order.OrderStatus == OrderStatus.StatusShipped)
                return (false, "The order has already been shipped. it can't be cancelled");

            order.OrderStatus = OrderStatus.StatusCancelled;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Item.UpdateSoldStatus(order.ItemId, false);

            await _unitOfWork.SaveAsync();

            return (true, "Order cancelled successfully");
        }


        private async Task SendOrderShippedEmail(string email, string itemName)
        {
            await _emailService.SendEmailAsync(email, "Your order has been shipped", $"<p>This email is to confirm that your order {itemName} has been shipped!</p>");
        }

    }

}

