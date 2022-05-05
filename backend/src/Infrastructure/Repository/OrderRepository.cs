using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Order order)
    {
        _db.Orders.Update(order);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderFromDb = _db.Orders.FirstOrDefault(x=> x.Id == id);
        if(orderFromDb == null)
            return;

        orderFromDb.OrderStatus = orderStatus;
        if(paymentStatus != null)
        {
            orderFromDb.PaymentStatus = paymentStatus;
        }
    }
}