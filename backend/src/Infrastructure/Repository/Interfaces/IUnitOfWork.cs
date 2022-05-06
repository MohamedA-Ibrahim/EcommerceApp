namespace Infrastructure.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IItemRepository Item { get; }
    IOrderDetailRepository OrderDetail { get; }
    IOrderRepository Order { get; }
    IUserAddressRepository UserAdress { get; }
    IAttributeTypeRepository AttributeType { get; }
    ICartRepository Cart { get; }
    Task SaveAsync();
}