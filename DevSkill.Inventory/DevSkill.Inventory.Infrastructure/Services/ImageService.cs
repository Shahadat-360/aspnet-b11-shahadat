using DevSkill.Inventory.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<string> SaveImageAsync(IFormFile ImageFile, string Folder)
        {
            if (ImageFile == null || ImageFile.Length == 0) return null;
            var UploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder);
            if(!Directory.Exists(UploadsFolder))
            {
                Directory.CreateDirectory(UploadsFolder);
            }

            var ImageExtension = Path.GetExtension(ImageFile.FileName);
            var UniqueImageName = $"{Guid.NewGuid()}{ImageExtension}";
            var UniqueImagePath = Path.Combine(UploadsFolder, UniqueImageName);
            using (var fileStream = new FileStream(UniqueImagePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }
            return Path.Combine(Folder,UniqueImageName).Replace("\\","/");
        }
        public Task<bool> DeleteImageAsync(string ImagePath)
        {
            if (string.IsNullOrWhiteSpace(ImagePath))
                return Task.FromResult(false);

            var SafePath = ImagePath.TrimStart('/', '\\');
            var FullPath = Path.Combine(_webHostEnvironment.WebRootPath, SafePath);

            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
