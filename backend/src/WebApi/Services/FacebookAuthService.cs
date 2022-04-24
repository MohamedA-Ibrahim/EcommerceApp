using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Settings;
using Newtonsoft.Json;

namespace WebApi.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private const string TokenValidationUrl ="https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private const string UserInfoUrl= "https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={0}";
        private readonly FacebookAuthSettings _facebookAuthSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public FacebookAuthService(FacebookAuthSettings facebookAuthSettings, IHttpClientFactory httpClientFactory)
        {
            _facebookAuthSettings = facebookAuthSettings;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
        {
            var formattedUrl = string.Format(TokenValidationUrl, accessToken,
                          _facebookAuthSettings.AppId,
                          _facebookAuthSettings.AppSecret);

            var responseAsString = await GetResponse(formattedUrl);
            return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(responseAsString);
        }

        public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            var formattedUrl = string.Format(UserInfoUrl, accessToken);

            //todo: handle exception
            string responseAsString = await GetResponse(formattedUrl);
            return JsonConvert.DeserializeObject<FacebookUserInfoResult>(responseAsString);
        }


        /// <summary>
        /// A helper method to send a request to an api and recieve the response
        /// </summary>
        /// <param name="formattedUrl">The url to send the request to</param>
        /// <returns>response</returns>
        private async Task<string> GetResponse(string formattedUrl)
        {
            var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();

            var responseAsString = await result.Content.ReadAsStringAsync();
            return responseAsString;
        }
    }
}
