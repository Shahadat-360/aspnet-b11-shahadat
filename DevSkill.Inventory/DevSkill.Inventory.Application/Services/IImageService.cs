using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile ImageFile, string Folder);
        Task<bool> DeleteImageAsync(string ImagePath);
    }
}
