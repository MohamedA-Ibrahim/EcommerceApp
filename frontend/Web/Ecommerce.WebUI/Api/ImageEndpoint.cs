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
            HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync("image", GetFileFromForm(file));
            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadAsAsync<string>();
            return item;
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
