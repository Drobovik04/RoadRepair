using ErrorOr;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Interfaces;

namespace RoadRepair.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public const string ROOT_DIR = "uploads"; 
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<ErrorOr<IFormFile>> GetFile(string fileName)
        {
            var fullPath = Path.Combine(_env.WebRootPath, ROOT_DIR, fileName);
            if (System.IO.File.Exists(fullPath))
            {
                using var stream = File.OpenRead(fullPath);
                return new FormFile(stream, 0, stream.Length, "File", fileName);
            }
            else
            {
                return Error.Failure(description: "File is not exists");
            }
        }

        public async Task<ErrorOr<string>> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Error.Failure(description: "File is null or length equal zero");

            var ext = Path.GetExtension(file.FileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            var uploadDir = Path.Combine(_env.WebRootPath, ROOT_DIR);

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var fullPath = Path.Combine(uploadDir, newFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return newFileName;
        }
        public async Task<ErrorOr<bool>> Delete(string name)
        {
            var fullPath = Path.Combine(_env.WebRootPath, ROOT_DIR, name);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                return true;
            }

            return false;
        }
        public async Task<ErrorOr<string>> Update(string oldFileName, IFormFile newFile)
        {
            var fullPath = Path.Combine(_env.WebRootPath, ROOT_DIR, oldFileName);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            var ext = Path.GetExtension(newFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            var uploadDir = Path.Combine(_env.WebRootPath, ROOT_DIR);

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var fullFilePath = Path.Combine(uploadDir, newFileName);

            using var stream = new FileStream(fullFilePath, FileMode.Create);
            await newFile.CopyToAsync(stream);

            return newFileName;
        }
    }
}
