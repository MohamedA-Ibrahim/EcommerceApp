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
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;
        private readonly IMapper _mapper;
        public UserAddressController(IMapper mapper, IUserAddressService userAddressService)
        {
            _mapper = mapper;
            _userAddressService = userAddressService;
        }


        [HttpGet(ApiRoutes.UserAddress.GetUserAddress)]
        public async Task<IActionResult> GetUserAddress()
        {
            UserAddress address = await _userAddressService.GetUserAddressAsync();

            var addressResponse = _mapper.Map<UserAddressResponse>(address);

            return Ok(addressResponse);
        }


        [HttpPost(ApiRoutes.UserAddress.Upsert)]
        public async Task<IActionResult> Upsert([FromBody] UpsertUserAddressRequest request)
        {
            var address = await _userAddressService.UpsertAsync(request);

            return Ok(_mapper.Map<UserAddressResponse>(address));
        }             
     
    }
}
