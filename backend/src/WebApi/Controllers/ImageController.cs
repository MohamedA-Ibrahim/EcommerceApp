using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    public class ImageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public ImageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Upload an image to the server
        /// </summary>
        /// <param name="file">The image to upload</param>
        /// <response code="200">Returns the url of the uploaded image</response>
        [Produces("text/plain")]
        [HttpPost(ApiRoutes.Images.Upload)]
        public async Task<string> UploadAsync([FromForm] IFormFile file)
        {
            var image = new FileDto
            {
                Content = file.OpenReadStream(),
                Name = file.FileName,
                ContentType = file.ContentType
            };

            var imageUrl = await _fileStorageService.UploadAsync(image);
            return imageUrl;
        }
    }

}