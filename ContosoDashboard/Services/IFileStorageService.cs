using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContosoDashboard.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string userId, int? projectId);
        Task<Stream> GetFileStreamAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
