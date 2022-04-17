
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;
using WebApi.Contracts.V1;
using System.Net.Mime;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpPost(ApiRoutes.Image.Upload)]
        public async Task<string> Upload([FromForm] IFormFile file)
        {
            try
            {
                if (file.Length <= 0)
                    return "Not Uploaded.";

                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    string path = _webHostEnvironment.WebRootPath + "\\image\\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + fileName + extension))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        return path + fileName + extension;
                    }

                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        
        [HttpGet(ApiRoutes.Image.Get)]
        public async Task<IActionResult> GetImageByName([FromRoute] string imageName)
        {
            string path = _webHostEnvironment.WebRootPath + "\\image\\";

            var filePath = Path.Combine(path, imageName);

            if (System.IO.File.Exists(filePath))
            {
                return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", imageName);
            }
            return NotFound();
        }

        /// <summary>
        /// Delete an image by its name
        /// </summary>
        /// <param name="imageName">The name of the image to delete</param>
        /// <returns>Deleted <paramref name="imageName"/></returns>
        [HttpDelete(ApiRoutes.Image.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string imageName)
        {
            string rootPath = _webHostEnvironment.WebRootPath + "\\image\\";

            var imagePath = Path.Combine(rootPath, imageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);

                return Ok($"Deleted {imageName}");
            }

            return NotFound();
        }
    }


}
