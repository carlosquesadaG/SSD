using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ContosoDashboard.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadRoot;

        public LocalFileStorageService(IWebHostEnvironment env)
        {
            _env = env;
            // Store outside wwwroot: AppData/uploads under content root or environment path
            _uploadRoot = Path.Combine(_env.ContentRootPath, "AppData", "uploads");
            if (!Directory.Exists(_uploadRoot))
            {
                Directory.CreateDirectory(_uploadRoot);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string userId, int? projectId)
        {
            if (file == null || file.Length == 0)
                raiseArgumentException("File is empty.");

            // Pattern: {userId}/{projectId or "personal"}/{uniqueId}.{extension}
            string projectFolder = projectId.HasValue ? projectId.Value.ToString() : "personal";
            string userFolderClean = string.IsNullOrEmpty(userId) ? "anonymous" : SanitizePath(userId);
            
            string targetDir = Path.Combine(_uploadRoot, userFolderClean, projectFolder);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            string extension = Path.GetExtension(file.FileName);
            string uniqueId = Guid.NewGuid().ToString();
            string uniqueFileName = $"{uniqueId}{extension}";
            string fullPath = Path.Combine(targetDir, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path from upload root or absolute path
            return fullPath;
        }

        public Task<Stream> GetFileStreamAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found on disk.", filePath);
            }

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(stream);
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        private void raiseArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        private string SanitizePath(string path)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                path = path.Replace(c, '_');
            }
            return path;
        }
    }
}
