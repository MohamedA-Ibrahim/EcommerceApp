using Domain.Entities;
using Web.Contracts.V1.Requests;

namespace Web.Services.DataServices.Interfaces
{
    public interface IOrderService
    {
        Task<Order> Get(int orderId);
        Task<List<Order>> GetBuyerOrders();
        Task<List<Order>> GetSellerOrders();
        Task<(Order order, string message)> CreateOrder(CreateOrderRequest orderRequest);

        Task<(bool success, string message)> CancelOrder(int orderId);
        Task<(bool success, string message)> ConfirmPayment(int orderId);
        Task<(bool success, string message)> ShipOrder(int orderId);
        Task<(bool success, string message)> StartProcessing(int orderId);
    }
}
