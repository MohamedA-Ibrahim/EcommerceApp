using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this email already exists" } };
            }

            var newUser = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                return new AuthenticationResult { Errors = result.Errors.Select(e => e.Description) };
            }

            return GenerateAuthenticationResult(newUser);

        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult { Errors = new[] { "User doesn't exist" } };
            }
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user,password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Password" } };
            }

            return GenerateAuthenticationResult(user);
        }

        private AuthenticationResult GenerateAuthenticationResult(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id
            };
        }

        public async Task<AuthenticationResult> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null ? await DeleteUserAsync(user) : new AuthenticationResult { Errors = null};
        }

        public async Task<AuthenticationResult> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return new AuthenticationResult { Errors = result.Errors.Select(e => e.Description) };
        }


    }
}

