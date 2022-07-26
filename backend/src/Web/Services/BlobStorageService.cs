using Application.Interfaces;
using Application.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Web.Services
{
    public class BlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<bool> DeleteAsync(string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("images");
            var blobClient = containerClient.GetBlobClient(filePath);
            if(blobClient == null)
                return false;
            await blobClient.DeleteIfExistsAsync();
            return true;
        }

        public async Task<string> UploadAsync(FileDto file)
        {
            if (file == null)
            {
                return null;
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient("ecommerce");
            var blobClient = containerClient.GetBlobClient(file.GetPathWithFileName());
            await blobClient.UploadAsync(file.Content, new BlobHttpHeaders { ContentType = file.ContentType });
            return blobClient.Uri.ToString();
        }
    }
}
