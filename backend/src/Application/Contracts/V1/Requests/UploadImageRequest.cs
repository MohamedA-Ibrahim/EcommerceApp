using Application.Models;

namespace Application.Contracts.V1.Requests
{
    public class UploadImageRequest
    {
        public FileDto File { get; set; }
    }
}
