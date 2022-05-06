﻿using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Item = new ItemRepository(_db);
        Order = new OrderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
        ShippingInfo = new ShippingInfoRepository(_db);
        AttributeType = new AttributeTypeRepository(_db);
    }

    public ICategoryRepository Category { get; }
    public IItemRepository Item { get; }
    public IOrderDetailRepository OrderDetail { get; }
    public IOrderRepository Order { get; }
    public IShippingInfoRepository ShippingInfo { get; }
    public IAttributeTypeRepository AttributeType { get; }

    public async Task SaveAsync()
    {
       await _db.SaveChangesAsync();
    }
}