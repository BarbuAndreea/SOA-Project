using Microsoft.AspNetCore.Http;
using MyDent.Domain.Models;
using System.Collections.Generic;

namespace MyDent.Services.Abstractions
{
    public interface IRadiographyService
    {
        List<string> SaveImages(IFormFileCollection images, string pathToSave);

        string SaveImage(IFormFile image, string pathToSave);

        void DeleteImageByPath(string fullPath);
    }
}
