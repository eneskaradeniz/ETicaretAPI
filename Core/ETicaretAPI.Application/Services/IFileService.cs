using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Application.Services
{
    public interface IFileService
    {
        Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files);
        string FileRename(string path, string fileName);
        Task<bool> CopyFileAsync(string path, IFormFile file);
    }
}
