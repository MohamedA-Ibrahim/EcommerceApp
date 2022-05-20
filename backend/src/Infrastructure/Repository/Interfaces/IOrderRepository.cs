using Domain.Entities;

namespace Infrastructure.Repository;

public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
    Task UpdateStatusAsync(int id, string Orderstatus, string? paymentStatus = null);
    Task<bool> UserIsOrderSellerAsync(int orderId, string userId);
    Task<bool> UserIsOrderSellerOrBuyerAsync(int orderId, string userId);

}