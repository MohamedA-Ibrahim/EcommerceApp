using Domain.Entities;
using Web.Contracts.V1.Requests;

namespace Web.Services.DataServices.Interfaces
{
    public interface IUserAddressService
    {
        Task<UserAddress> GetUserAddressAsync();
        Task<UserAddress> UpsertAsync(UpsertUserAddressRequest request);
    }
}
