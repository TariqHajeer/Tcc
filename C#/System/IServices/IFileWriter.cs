using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IServices
{
    public interface IFileWriter
    {
        bool UploadFile(IFormFile file, string folderName, string fileName,out string newPath);
    }
}
