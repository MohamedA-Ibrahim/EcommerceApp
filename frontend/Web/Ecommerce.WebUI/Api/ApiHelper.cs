using Ecommerce.WebUI.Models.User;
using System.Net.Http.Headers;

namespace Ecommerce.WebUI.Api
{
    public class ApiHelper : IApiHelper
    {        
        private HttpClient _apiClient;
        private IAuthenticatedUser _authenticatedUser;

        public ApiHelper(IAuthenticatedUser authenticatedUser)
        {
            InitializeClient();
            _authenticatedUser = authenticatedUser;
        }
        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        private void InitializeClient()
        {
            //string serverUrl = "https://localhost:7243/api/v1/";
            string serverUrl = "https://ecommeapi.azurewebsites.net/api/v1/";

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(serverUrl);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)

            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/identity", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Autorization", $"Bearer { token}");

            using (HttpResponseMessage response = await _apiClient.GetAsync("Identity"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    _authenticatedUser.CreatedDate = result.CreatedDate;
                    _authenticatedUser.EmailAddress = result.EmailAddress;
                    _authenticatedUser.FirstName = result.FirstName;
                    _authenticatedUser.Id = result.Id;
                    _authenticatedUser.LastName = result.LastName;
                    _authenticatedUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }

}
