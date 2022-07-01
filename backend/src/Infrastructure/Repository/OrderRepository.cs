using Application.Enums;
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

    public async Task UpdateStatusAsync(int id, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var orderFromDb = await _db.Orders.FindAsync(id);
        if (orderFromDb == null)
            return;

        orderFromDb.OrderStatus = orderStatus;
        orderFromDb.PaymentStatus = paymentStatus;
        
    }

    public async Task<bool> UserIsOrderSellerAsync(int orderId, string userId)
    {
        return await _db.Orders.AnyAsync(x=> x.Id == orderId && x.Item.SellerId == userId);
    }

    public async Task<bool> UserIsOrderBuyerAsync(int orderId, string userId)
    {
        return await _db.Orders.AnyAsync(x => x.Id == orderId && x.BuyerId == userId);

    }

    public async Task<bool> UserHasExistingOrderForItem(int itemId, string userId)
    {
        return await _db.Orders.AnyAsync(x => x.ItemId == itemId && x.BuyerId == userId);
    }
}