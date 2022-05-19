using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository;

public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
{
    private readonly ApplicationDbContext _db;

    public UserAddressRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(UserAddress userAddress)
    {
        _db.UserAddresses.Update(userAddress);
    }
}