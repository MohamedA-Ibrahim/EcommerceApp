using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> DeleteUserAsync(string userId);
    }
}
