namespace Infrastructure.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IItemRepository Item { get; }
    IOrderDetailRepository OrderDetail { get; }
    IOrderRepository Order { get; }
    IShippingInfoRepository ShippingInfo { get; }
    IAttributeTypeRepository AttributeType { get; }
    Task SaveAsync();
}