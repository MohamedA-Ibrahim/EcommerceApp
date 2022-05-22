using Application.Models;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(FileDto file);
    }
}
