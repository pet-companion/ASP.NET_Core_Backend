using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetCareCore.Constant;
using PetCareCore.Dto;
using PetCareCore.Enum;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            //foreach (var pet in petVM)
            //{
            //    pet.PetImage = await _fileService.GetFile(pet.ImgName, FileFolder.FolderName);
            //}
            return new APIResponse<List<PetVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Pets List", petVM.Count(), petVM);
        }

        public async Task<APIResponse<PetVM>> GetPet(int petId)
        {
            var pet = await _dbContext.Pets
                .FindAsync(petId);
            if (pet is null)
            {
                return new APIResponse<PetVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Pet Not Found", null);
            }
            var petVM = _Mapper.Map<PetVM>(pet);
            //petVM.PetImage = await _fileService.GetFile(pet.ImgName, FileFolder.FolderName);
            return new APIResponse<PetVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Pet Data", 1, petVM);
        }

        public async Task<APIResponse> AddPet(AddPetDto petData)
        {
            if (petData is null || string.IsNullOrWhiteSpace(petData.Name) || petData.UserId == 0 || petData.BreedId == 0)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            //check if UserId and BreedId found
            var user = _dbContext.Users.Find(petData.UserId);
            var breed = _dbContext.Breeds.Find(petData.BreedId);
            if (user is null || breed is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Or Breed Not Found");
            }
            var pet = _Mapper.Map<Pet>(petData);
            if (petData.PetImg?.Length > 0)
            {
                pet.ImgName = await _fileService.SaveFile(petData.PetImg, FileFolder.FolderName);
            }
            await _dbContext.Pets.AddAsync(pet);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Pet Data Added Successfully");
        }

        public async Task<APIResponse> UpdatePet(UpdatePetDto petData)
        {
            if (petData is null || petData.Id is null || string.IsNullOrWhiteSpace(petData.Name) || petData.UserId == 0 || petData.BreedId == 0)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            //check if UserId and BreedId found
            var user = _dbContext.Users.Find(petData.UserId);
            var breed = _dbContext.Breeds.Find(petData.BreedId);
            if (user is null || breed is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Or Breed Not Found");
            }
            var dbPet = await _dbContext.Pets.FindAsync(petData.Id);
            if (dbPet is null)
            {
                return new APIResponse<StoreVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Pet Not Found", null);
            }
            _Mapper.Map(petData, dbPet);

            if (petData.PetImg?.Length > 0)
            {
                dbPet.ImgName = await _fileService.SaveFile(petData.PetImg, FileFolder.FolderName);
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Pet Data Updated Successfully");
        }

        public async Task<APIResponse> DeletePet(int petId)
        {
            var pet = await _dbContext.Pets
                .FindAsync(petId);
            if (pet is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Pet Not Found");
            }
            pet.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Pet Data Deleted Successfully");
        }
    }
}
