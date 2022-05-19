using Domain.Entities;

namespace Infrastructure.Repository;

public interface IUserAddressRepository : IRepository<UserAddress>
{
    void Update(UserAddress userAddress);
}