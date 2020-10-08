using System;
using System.Collections.Generic;
using System.IO;
using System.IServices;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Static;
namespace System.Services
{
    public class ImageWriter : IFileWriter
    {
        public bool UploadFile(IFormFile file, string folderName, string fileName, out string newPath)
        {
            if (IsImageValid(file))
            {
                newPath = WritImage(file, folderName, fileName);
                return true;
            }

            newPath = "";
            return false;
        }

        private bool IsImageValid(IFormFile image)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
        }
        private string WritImage(IFormFile image, string folderName, string fileName)
        {
            string mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\");
            var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];



            CreateFolder(mainPath, folderName);
            fileName = SetNameForFile(folderName, fileName, extension);
            fileName += extension;
            var path = Path.Combine(mainPath, folderName, fileName);
            //var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folderName}", fileName);
            using (var bits = new FileStream(path, FileMode.Create))
            {
                image.CopyToAsync(bits);
            }
            return Path.Combine(mainPath, folderName, fileName);
        }
        private string SetNameForFile(string folderName, string fileName, string extension, int counter = 0)
        {
            string newFileName = fileName;
            if (counter != 0)
            {
                newFileName += $"({counter})";
            }
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + folderName + "\\" + newFileName + extension)))
            {
                return SetNameForFile(folderName, fileName, extension, ++counter);
            }
            return newFileName;
        }
        private void CreateFolder(string path, string folderName)
        {
            if (Directory.Exists(Path.Combine(path, folderName)))
            {
                return;
            }
            Directory.CreateDirectory(Path.Combine(path, folderName));
        }
    }
}
