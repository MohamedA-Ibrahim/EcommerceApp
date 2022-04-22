using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;

namespace WebApi.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">the user email and password</param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> RegisterAsync([FromBody]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="request">The user email and password</param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
           
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
    }
}
