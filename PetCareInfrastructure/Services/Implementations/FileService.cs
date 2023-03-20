using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PetCareCore.ViewModel;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFile(IFormFile File, string FolderName)
        {
            var MainPath = Path.Combine(_env.ContentRootPath, FolderName);
            var FileName = string.Empty;
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }
            if (File != null && File.Length > 0)
            {
                FileName = Path.GetFileNameWithoutExtension(File.FileName).Replace(" ", "") + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + Path.GetExtension(File.FileName);
                await using var FileStream = new FileStream(Path.Combine(MainPath, FileName), FileMode.Create);
                await File.CopyToAsync(FileStream);
            }
            return FileName;
        }

        public Task<string> GetFilePath(string fileName, string folderName)
        {
            string filePath = string.Empty;
            if (fileName != null && folderName != null)
            {
                var MainPath = Path.Combine(_env.ContentRootPath, folderName);
                filePath = Path.Combine(MainPath, fileName);
            }
            return Task.FromResult(filePath);
        }

        public async Task<FileData> GetFile(string fileName, string folderName)
        {
            try
            {
                string filePath = string.Empty;
                if (fileName != null && folderName != null)
                {
                    var MainPath = Path.Combine(_env.ContentRootPath, folderName);
                    filePath = Path.Combine(MainPath, fileName);
                }
                if (!File.Exists(filePath))
                {
                    return null;
                }
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                return new FileData
                {
                    FileName = fileName,
                    FileContent = fileBytes
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
