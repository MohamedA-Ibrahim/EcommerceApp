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
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.Item.SellerId == _currentUserService.UserId, null, x => x.Item, x=> x.Item.Seller);

            return orders;
        }

        public async Task<List<Order>> GetBuyerOrdersAsync()
        {
            //Get all buyer orders except orders cancelled by the buyer
            var orders = await _unitOfWork.Order.GetAllIncludingAsync(x => x.BuyerId == _currentUserService.UserId && x.OrderStatus != OrderStatus.Cancelled,
                null, x => x.Buyer, x => x.Item, x => x.Item.Seller);

            return orders;
        } 

        public async Task<Order> GetAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.Buyer, x => x.Item, x=> x.Item.Seller);
            return order;
        }

        public async Task<(Order order, string message)> CreateOrderAsync(CreateOrderRequest orderRequest)
        {
            var userOwnsItem = await _unitOfWork.Item.UserOwnsItemAsync(orderRequest.ItemId, _currentUserService.UserId);

            if (userOwnsItem)
                return (null, "You can't buy your own item!");

            bool userHasExistingOrderForItem = await _unitOfWork.Order.UserHasExistingOrderForItem(orderRequest.ItemId, _currentUserService.UserId);
            if(userHasExistingOrderForItem)
                return (null, "You can't buy the same item twice!");

            var order = new Order
            {
                ItemId = orderRequest.ItemId,
                PhoneNumber = orderRequest.PhoneNumber,
                StreetAddress = orderRequest.StreetAddress,
                City = orderRequest.City,
                RecieverName = orderRequest.RecieverName,
                OrderDate = DateUtil.GetCurrentDate(),
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };


            await _unitOfWork.Order.AddAsync(order);

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

            order.OrderStatus = OrderStatus.InProcess;
            order.PaymentStatus = PaymentStatus.Pending;

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

            order.OrderStatus = OrderStatus.Approved;
            order.PaymentStatus = PaymentStatus.Approved;
            order.PaymentDate = DateUtil.GetCurrentDate();

            _unitOfWork.Order.Update(order);

            //Set the item to sold
            _unitOfWork.Item.UpdateSoldStatus(order.ItemId, true);

            //Set all remaining orders to rejected when the seller accepts one
            var remainingOrders = await _unitOfWork.Order.GetAllAsync(x=> x.Id != order.Id && x.ItemId == order.ItemId);
            remainingOrders.ForEach(x=> x.OrderStatus = OrderStatus.Rejected);

            await _unitOfWork.SaveAsync();

            return (true, "Order payment status updated successfully");
        }

        public async Task<(bool success, string message)> ShipOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultIncludingAsync(orderId, x => x.Item, x=> x.Item.Seller);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not the seller of this order");

            if (order.OrderStatus == OrderStatus.Shipped)
                return (false, "The order has already been shipped.");

            if (order.PaymentStatus != PaymentStatus.Approved)
                return (false, "The order is still not paid!");

            order.OrderStatus = OrderStatus.Shipped;
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

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderBuyerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not a buyer of this order");

            if (order.OrderStatus == OrderStatus.Shipped)
                return (false, "The order has already been shipped. it can't be cancelled");

            order.OrderStatus = OrderStatus.Cancelled;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Item.UpdateSoldStatus(order.ItemId, false);

            await _unitOfWork.SaveAsync();

            return (true, "Order cancelled successfully");
        }

        public async Task<(bool success, string message)> RejectOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(orderId);
            if (order == null)
                return (false, "Order doesn't exist");

            var userOwnsOrder = await _unitOfWork.Order.UserIsOrderSellerAsync(orderId, _currentUserService.UserId);
            if (!userOwnsOrder)
                return (false, "You are not the seller for this order");

            if (order.OrderStatus == OrderStatus.Shipped)
                return (false, "The order has already been shipped. it can't be rejected");

            order.OrderStatus = OrderStatus.Rejected;

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

