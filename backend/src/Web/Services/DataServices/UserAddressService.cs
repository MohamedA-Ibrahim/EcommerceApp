using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Web.Contracts.V1.Requests;
using Web.Services.DataServices.Interfaces;

namespace Web.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UserAddressService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<UserAddress> GetUserAddressAsync()
        {
            return await _unitOfWork.UserAddress.FindByAsync(x => x.CreatedBy == _currentUserService.UserId);
        }

        public async Task<UserAddress> UpsertAsync(UpsertUserAddressRequest request)
        {
            var existingAddress = await _unitOfWork.UserAddress.FindByAsync(x => x.CreatedBy == _currentUserService.UserId);

            //Saving address for the first time
            if (existingAddress == null)
            {
                var address = new UserAddress
                {
                    PhoneNumber = request.PhoneNumber,
                    StreetAddress = request.StreetAddress,
                    City = request.City,
                    RecieverName = request.RecieverName
                };

                await _unitOfWork.UserAddress.AddAsync(address);
                await _unitOfWork.SaveAsync();
                return address;
            }

            //updating an Address
            existingAddress.PhoneNumber = request.PhoneNumber;
            existingAddress.StreetAddress = request.StreetAddress;
            existingAddress.City = request.City;
            existingAddress.RecieverName = request.RecieverName;
            _unitOfWork.UserAddress.Update(existingAddress);
            await _unitOfWork.SaveAsync();
            return existingAddress;

        }
    }
}
