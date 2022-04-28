using Application.Models;

namespace WebApi.Contracts.V1.Requests
{
    public class UploadImageRequest
    {
        public FileDto File { get; set; }
    }
}
