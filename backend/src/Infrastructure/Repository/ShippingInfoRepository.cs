using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class ShippingInfoRepository : Repository<ShippingInfo>, IShippingInfoRepository
{
    private readonly ApplicationDbContext _db;

    public ShippingInfoRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ShippingInfo shippingInfo)
    {
        _db.ShippingInfos.Update(shippingInfo);
    }
}