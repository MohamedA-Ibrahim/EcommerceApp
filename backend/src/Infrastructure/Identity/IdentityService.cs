using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Application.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly ApplicationDbContext _dataContext;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFacebookAuthService _facebookAuthService;
    public IdentityService(
        UserManager<ApplicationUser> userManager,
        JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, ApplicationDbContext dataContext, IFacebookAuthService facebookAuthService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _dataContext = dataContext;
        _facebookAuthService = facebookAuthService;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return new AuthenticationResult {Errors = new[] {"User with this email already exists"}};

        var newUser = new ApplicationUser
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(newUser, password);

        if (!result.Succeeded) return new AuthenticationResult {Errors = result.Errors.Select(e => e.Description)};

        return await GenerateAuthenticationResultAsync(newUser);
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return new AuthenticationResult {Errors = new[] {"User doesn't exist"}};
        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!userHasValidPassword) return new AuthenticationResult {Errors = new[] {"Invalid Password"}};

        return await GenerateAuthenticationResultAsync(user);
    }

    public async Task<AuthenticationResult> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null
            ? await DeleteUserAsync(user)
            : new AuthenticationResult {Errors = new[] {"User not found"}};
    }
    public async Task<AuthenticationResult> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return new AuthenticationResult { Errors = result.Errors.Select(e => e.Description) };
    }
    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);

        if (validatedToken == null) return new AuthenticationResult {Errors = new[] {"Invalid Token"}};

        //This part is optional to prevent refreshing
        //the token when it isn't expired.
        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            return new AuthenticationResult {Errors = new[] {"This Token hasn't expired yet"}};

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

        //Some Validations
        if (storedRefreshToken == null)
            return new AuthenticationResult {Errors = new[] {"This refresh token doesn't exist"}};

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            return new AuthenticationResult {Errors = new[] {"This refresh token has expired"}};

        if (storedRefreshToken.Invalidated)
            return new AuthenticationResult {Errors = new[] {"This refresh token has been invalidted"}};

        if (storedRefreshToken.Used)
            return new AuthenticationResult {Errors = new[] {"This refresh token has been used"}};
        if (storedRefreshToken.JwtId != jti)
            return new AuthenticationResult {Errors = new[] {"This refresh token doesn't match this JWT"}};

        //update the refresh token used status in database
        storedRefreshToken.Used = true;
        _dataContext.RefreshTokens.Update(storedRefreshToken);
        await _dataContext.SaveChangesAsync();


        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "userId").Value);
        return await GenerateAuthenticationResultAsync(user);
    }

    public async Task<AuthenticationResult> LoginWithFacebookAsync(string accessToken)
    {
        var validatedTokenResult = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);
        if (!validatedTokenResult.Data.IsValid)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "Invalid Facebook token" }
            };
        }

        var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

        var user = await _userManager.FindByEmailAsync(userInfo.Email);

        //User is already registered with email/facebook
        if (user != null)
        {
            return await GenerateAuthenticationResultAsync(user);
        }

        var identityUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = userInfo.Email,
            UserName = userInfo.Email,
        };

        var createdResult = await _userManager.CreateAsync(identityUser);
        if (!createdResult.Succeeded)
        {
            return new AuthenticationResult
            {
                //Error is vague in purpose
                Errors = new[] { "Something went wrong" }
            };
        }

        return await GenerateAuthenticationResultAsync(identityUser);
    }

    #region Token Helpers

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("userId", user.Id)
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.Add(_jwtSettings.TokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6)
        };

        await _dataContext.RefreshTokens.AddAsync(refreshToken);
        await _dataContext.SaveChangesAsync();

        return new AuthenticationResult
        {
            Success = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken.Token
        };
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion
}