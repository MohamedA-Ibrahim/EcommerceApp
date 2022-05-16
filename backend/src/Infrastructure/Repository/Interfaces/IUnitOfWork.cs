namespace Infrastructure.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IItemRepository Item { get; }
    IOrderRepository Order { get; }
    IUserAddressRepository UserAddress { get; }
    IAttributeTypeRepository AttributeType { get; }
    Task SaveAsync();
}