using Domain.Entities;
using Web.Contracts.V1.Requests;

namespace Web.Services.DataServices.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetAsync(int orderId);
        Task<List<Order>> GetBuyerOrdersAsync();
        Task<List<Order>> GetSellerOrdersAsync();
        Task<(Order order, string message)> CreateOrderAsync(CreateOrderRequest orderRequest);

        Task<(bool success, string message)> CancelOrderAsync(int orderId);
        Task<(bool success, string message)> RejectOrderAsync(int orderId);

        Task<(bool success, string message)> ConfirmPaymentAsync(int orderId);
        Task<(bool success, string message)> ShipOrderAsync(int orderId);
        Task<(bool success, string message)> StartProcessingAsync(int orderId);
    }
}
