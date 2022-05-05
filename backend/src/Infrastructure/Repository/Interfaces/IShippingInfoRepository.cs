using Domain.Entities;

namespace Infrastructure.Repository;

public interface IShippingInfoRepository : IRepository<ShippingInfo>
{
    void Update(ShippingInfo shippingInfo);
}