using LearningManagementSystem.Application.Abstractions.Services.Storage;
using Microsoft.AspNetCore.Hosting;
using LearningManagementSystem.Application.Abstractions.Services.Aws;

namespace LearningManagementSystem.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : IStorage
    {
        private readonly IWebHostEnvironment _environment;

        public LocalStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async ValueTask<string> GetFileUrlAsync(string fileName, string prefix)
        {
            // Generate the file path for local storage
            var localPath = _environment.ContentRootPath;
            var filePath = Path.Combine(localPath, "Documents", prefix, fileName);

            // Return the file URL or path
            return Path.Combine(localPath, "Documents", prefix, fileName);
        }

        public async ValueTask<string> UploadFileAsync(FileRequest request)
        {
            var localPath = _environment.ContentRootPath;
            var directoryPath = Path.Combine("Documents", request.Prefix, request.FileName);
            var fullPath = Path.Combine(localPath, directoryPath);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(fullPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Upload the file
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                await request.File.CopyToAsync(fs);
            }

            return fullPath; // Or return a relative path or URL
        }

        public async ValueTask<string> UpdateFileAsync(FileRequest request, string fileName)
        {
            var localPath = _environment.ContentRootPath;
            var directoryPath = Path.Combine("Documents", request.Prefix);
            var fullPath = Path.Combine(localPath, directoryPath, fileName);

            // Check if the file exists and delete it
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath); // Delete the old file
            }

            // Upload the new file
            return await UploadFileAsync(request);
        }
    }
}
