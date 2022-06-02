using Application.Enums;
using Domain.Entities;

namespace Infrastructure.Repository;

public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
    Task UpdateStatusAsync(int id, OrderStatus orderStatus, PaymentStatus paymentStatus);
    Task<bool> UserIsOrderSellerAsync(int orderId, string userId);
    Task<bool> UserIsOrderBuyerAsync(int orderId, string userId);

}