using Ecommerce.WebUI.Models;
using System.Net;

namespace Ecommerce.WebUI.Api
{
    public interface IImageEndpoint
    {
        Task<HttpStatusCode> DeleteImage(string imageName);
        public Task<string> UploadImage(IFormFile file);
    }
}