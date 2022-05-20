using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderFromDb = await _db.Orders.FindAsync(id);
        if(orderFromDb == null)
            return;

        orderFromDb.OrderStatus = orderStatus;
        if(paymentStatus != null)
        {
            orderFromDb.PaymentStatus = paymentStatus;
        }
    }

    public async Task<bool> UserIsOrderSellerAsync(int orderId, string userId)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order == null)
            return false;

        if (order.SellerId != userId) 
            return false;

        return true;
    }

    public async Task<bool> UserIsOrderSellerOrBuyerAsync(int orderId, string userId)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order == null)
            return false;

        if (order.SellerId != userId && order.BuyerId != userId)
            return false;

        return true;
    }
}