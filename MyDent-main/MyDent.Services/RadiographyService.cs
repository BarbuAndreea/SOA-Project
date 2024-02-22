using Microsoft.AspNetCore.Http;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services
{
    public class RadiographyService : IRadiographyService
    {
        private readonly Random _randomNumberGenerator = new();

        public List<string> SaveImages(IFormFileCollection images, string pathToSave)
        {
            if (images.Count <= 0)
            {
                throw new Exception("No images!");
            }
            return images.Select(image => SaveImage(image, pathToSave)).ToList();
        }

        public string SaveImage(IFormFile image, string pathToSave)
        {
            if (image.Length <= 0)
            {
                throw new Exception("The image is empty!");
            }

            try
            {
                string fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.TrimStart('\"').TrimEnd('\"');
                string type = System.IO.Path.GetExtension(fileName);
                string imageName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                string imageFinalName = ChangeImageName(imageName, type, pathToSave, out var fullPath);
                using var stream = new FileStream(fullPath, FileMode.Create);
                image.CopyTo(stream);
                return imageFinalName;
            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        private string ChangeImageName(string imageName, string fileType, string pathToSave, out string fullPath)
        {
            string newImageName = imageName + DateTime.Now.Ticks;
            fullPath = Path.Combine(pathToSave, newImageName + fileType);
            string temporaryImageName = "";
            while (File.Exists(fullPath))
            {
                int number = _randomNumberGenerator.Next();
                temporaryImageName = newImageName + number;
                fullPath = Path.Combine(pathToSave, temporaryImageName + fileType);
            }
            if (temporaryImageName != "" && newImageName != temporaryImageName)
            {
                return temporaryImageName + fileType;
            }
            return newImageName + fileType;
        }

        public void DeleteImageByPath(string fullPath)
        {
            File.Delete(fullPath);
        }
    }
}
