using Ecommerce.WebUI.Models;

namespace Ecommerce.WebUI.Api
{
    public interface IImageEndpoint
    {
        public Task<string> UploadImage(IFormFile file);
    }
}
