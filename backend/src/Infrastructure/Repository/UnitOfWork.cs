using Infrastructure.Persistence;

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
        UserAddress = new UserAddressRepository(_db);
        AttributeType = new AttributeTypeRepository(_db);
        AttributeValue = new AttributeValueRepository(_db);
    }

    public ICategoryRepository Category { get; }
    public IItemRepository Item { get; }
    public IOrderRepository Order { get; }
    public IUserAddressRepository UserAddress { get; }
    public IAttributeTypeRepository AttributeType { get; }
    public IAttributeValueRepository AttributeValue { get; }

    public async Task SaveAsync()
    {
       await _db.SaveChangesAsync();
    }
}