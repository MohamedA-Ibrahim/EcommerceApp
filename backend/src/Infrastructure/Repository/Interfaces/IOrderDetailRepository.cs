using Domain.Entities;

namespace Infrastructure.Repository;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail orderDetail);
}