using Domain.Entities;

namespace Infrastructure.Repository;

public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
    void UpdateStatus(int id, string Orderstatus, string? paymentStatus = null);
}