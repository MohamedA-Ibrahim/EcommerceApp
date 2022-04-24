using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;
using WebApi.Contracts.V1.Requests;
using WebApi.Contracts.V1.Responses;

namespace WebApi.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    ///     Register user
    /// </summary>
    /// <param name="request">the user email and password</param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthFailedResponse
            {
                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
            });

        var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    /// <summary>
    ///     Login a user
    /// </summary>
    /// <param name="request">The user email and password</param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Identity.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
    {
        var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    /// <summary>
    ///     Refresh the user token so he isn't required to keep logging again each time
    /// </summary>
    /// <param name="request"></param>
    /// <returns>the token and refresh token</returns>
    [HttpPost(ApiRoutes.Identity.Refresh)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }
}