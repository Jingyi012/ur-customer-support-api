using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string fileName, string folder)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            string sanitizedFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()))
                                      .Trim().Replace(" ", "_"); ;

            if (string.IsNullOrWhiteSpace(sanitizedFileName))
            {
                sanitizedFileName = "file";
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);

            string uniqueFileName = $"{timestamp}_{sanitizedFileName}_{shortGuid}{fileExtension}";

            string safeFolder = folder.TrimStart('/');

            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, safeFolder);
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{safeFolder}/{uniqueFileName}";
        }

        public async Task DeleteFileAsync(string filePath)
        {
            string absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}
