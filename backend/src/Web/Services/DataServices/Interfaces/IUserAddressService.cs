using Domain.Entities;
using Web.Contracts.V1.Requests;

namespace Web.Services.DataServices.Interfaces
{
    public interface IUserAddressService
    {
        Task<UserAddress> GetUserAddress();
        Task<UserAddress> Upsert(UpsertUserAddressRequest request);
    }
}
