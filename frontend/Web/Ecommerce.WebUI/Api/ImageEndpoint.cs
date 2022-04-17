using System.Net;

namespace Ecommerce.WebUI.Api
{
    public class ImageEndpoint : IImageEndpoint
    {
        private IApiHelper _apiHelper;
        public ImageEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync("images", GetFileFromForm(file));
            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadAsAsync<string>();
            return item;
        }

        public async Task<HttpStatusCode> DeleteImage(string imageName)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"images/{imageName}");
            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }

        private static MultipartFormDataContent GetFileFromForm(IFormFile file)
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);
            return multiContent;
        }
    }
}
