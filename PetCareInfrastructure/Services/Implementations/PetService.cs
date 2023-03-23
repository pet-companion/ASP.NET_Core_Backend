using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Constant;
using PetCareCore.Dto;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class PetService : IPetService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;
        private readonly IFileService _fileService;

        public PetService(PetCareDbContext dbContext, IMapper mapper, IFileService fileService)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
            _fileService = fileService;
        }

        public async Task<APIResponse<List<PetVM>>> GetPetList()
        {
            var petList = await _dbContext.Pets.ToListAsync();
            var petVM = _Mapper.Map<List<PetVM>>(petList);
            foreach (var pet in petVM)
            {
                pet.PetImage = await _fileService.GetFile(pet.ImgName, FileFolder.FolderName);
            }
            return new APIResponse<List<PetVM>>(true, "All Pet List", petVM, petVM.Count());
        }

        public async Task<APIResponse<PetVM>> GetPet(int petId)
        {
            var pet = await _dbContext.Pets
                .FindAsync(petId);
            if (pet is null)
            {
                return new APIResponse<PetVM>(false, "Pet Not Found", null, 0);
            }
            var petVM = _Mapper.Map<PetVM>(pet);
            petVM.PetImage = await _fileService.GetFile(pet.ImgName, FileFolder.FolderName);
            return new APIResponse<PetVM>(true, "Pet Data", petVM, 1);
        }

        public async Task<APIResponse> AddUpdatePet(AddUpdatePetDto petData)
        {
            var pet = new Pet();
            var msg = string.Empty;
            if (petData is null || string.IsNullOrWhiteSpace(petData.Name) || petData.UserId == 0 || petData.BreedId == 0)
            {
                return new APIResponse(false, "All Fields Are Required");
            }
            //check if UserId and BreedId found
            var user = _dbContext.Users.Find(petData.UserId);
            var breed = _dbContext.Breeds.Find(petData.BreedId);
            if (user is null || breed is null)
            {
                return new APIResponse(false, "User Or Breed Not Found");
            }
            //Edit
            if (petData.Id != null)
            {
                pet = await _dbContext.Pets.FindAsync(petData.Id);
                if (pet is null)
                {
                    return new APIResponse(false, "Pet Not Found");
                }
                pet.LastUpdatedAt = DateTime.Now;
                msg = "Pet Updated Successfully";
            }
            pet.Name = petData.Name;
            pet.Weight = petData.Weight;
            pet.UserId = petData.UserId;
            pet.BreedId = petData.BreedId;

            if (petData.PetImg.Length > 0)
            {
                pet.ImgName = await _fileService.SaveFile(petData.PetImg, FileFolder.FolderName);
            }

            if (petData.Id == null)
            {
                await _dbContext.Pets.AddAsync(pet);
                msg = "Pet Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeletePet(int petId)
        {
            var pet = await _dbContext.Pets
                .FindAsync(petId);
            if (pet is null)
            {
                return new APIResponse(false, "Pet Not Found");
            }
            pet.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Pet Deleted Successfully");
        }
    }
}
