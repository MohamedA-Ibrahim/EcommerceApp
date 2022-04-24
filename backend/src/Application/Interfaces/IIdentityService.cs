using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> DeleteUserAsync(string userId);
    Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    Task<AuthenticationResult> LoginWithFacebookAsync(string accessToken);
}