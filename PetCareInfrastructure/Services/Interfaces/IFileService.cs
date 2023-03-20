using Microsoft.AspNetCore.Http;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile File, string FolderName);
        Task<string> GetFilePath(string fileName, string folderName);
        Task<FileData> GetFile(string fileName, string folderName);
    }
}
