using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web.Contracts.V1;
using Web.Contracts.V1.Requests;
using Web.Contracts.V1.Responses;

namespace Web.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserAddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public UserAddressController(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }


        [HttpGet(ApiRoutes.UserAddress.GetUserAddress)]
        public async Task<IActionResult> GetUserAddress()
        {
            var address = await _unitOfWork.UserAddress.FindByAsync(x => x.CreatedBy == _currentUserService.UserId);

            var addressResponse = _mapper.Map<UserAddressResponse>(address);

            return Ok(addressResponse);
        }

        [HttpPost(ApiRoutes.UserAddress.Upsert)]
        public async Task<IActionResult> Upsert([FromBody] UpsertUserAddressRequest request)
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
                return Ok(_mapper.Map<UserAddressResponse>(address));
            }

            //updating an Address
            existingAddress.PhoneNumber = request.PhoneNumber;
            existingAddress.StreetAddress = request.StreetAddress;
            existingAddress.City = request.City;
            existingAddress.RecieverName = request.RecieverName;
            _unitOfWork.UserAddress.Update(existingAddress);
            await _unitOfWork.SaveAsync();

            return Ok((_mapper.Map<UserAddressResponse>(existingAddress)));
        }

        [HttpDelete(ApiRoutes.UserAddress.Delete)]
        public async Task<IActionResult> Delete()
        {
            var userAddress = await _unitOfWork.UserAddress.FindByAsync(x => x.CreatedBy == _currentUserService.UserId);

            if (userAddress == null)
                return NotFound();

            _unitOfWork.UserAddress.Remove(userAddress);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
