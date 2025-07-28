using ErrorOr;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces
{
    public interface IFileService
    {
        const string ROOT_DIR = "uploads";

        Task<ErrorOr<IFormFile>> GetFile(string path);
        Task<ErrorOr<string>> Upload(IFormFile file);
        Task<ErrorOr<bool>> Delete(string name);
        Task<ErrorOr<string>> Update(string oldFileName, IFormFile newFile);
    }
}
