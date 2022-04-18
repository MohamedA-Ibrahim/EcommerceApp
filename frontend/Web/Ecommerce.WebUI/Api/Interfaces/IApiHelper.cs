using Ecommerce.WebUI.Models.User;

namespace Ecommerce.WebUI.Api
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}
