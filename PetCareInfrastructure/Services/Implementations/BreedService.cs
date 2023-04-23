using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class BreedService : IBreedService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;

        public BreedService(PetCareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
        }

        public async Task<APIResponse<List<BreedVM>>> GetBreedList()
        {
            var breedList = await _dbContext.Breeds.ToListAsync();
            var breedVM = _Mapper.Map<List<BreedVM>>(breedList);
            return new APIResponse<List<BreedVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Breeds Categories", breedVM.Count(), breedVM);

        }

        public async Task<APIResponse<BreedVM>> GetBreed(int breedId)
        {
            var breed = await _dbContext.Breeds
                .FindAsync(breedId);
            if (breed is null)
            {
                return new APIResponse<BreedVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Breed Not Found", null);

            }
            var breedVM = _Mapper.Map<BreedVM>(breed);
            return new APIResponse<BreedVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Breed Data", 1, breedVM);
        }

        public async Task<APIResponse> AddBreed(AddBreedDto breedData)
        {
            var breed = new Breed();
            if (breedData is null || string.IsNullOrWhiteSpace(breedData.Name) || string.IsNullOrWhiteSpace(breedData.Description))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            breed.Name = breedData.Name;
            breed.Description = breedData.Description;
            await _dbContext.Breeds.AddAsync(breed);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Breed Added Successfully");
        }

        public async Task<APIResponse> UpdateBreed(UpdateBreedDto breedData)
        {
            if (breedData is null || string.IsNullOrWhiteSpace(breedData.Name) || string.IsNullOrWhiteSpace(breedData.Description))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            var dbBreed = await _dbContext.Breeds.FindAsync(breedData.Id);
            if (dbBreed is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Breed Not Found");
            }
            dbBreed.Name = breedData.Name;
            dbBreed.Description = breedData.Description;
            dbBreed.LastUpdatedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Breed Updated Successfully");
        }

        public async Task<APIResponse> DeleteBreed(int breedId)
        {
            var breed = await _dbContext.Breeds
                .FindAsync(breedId);
            if (breed is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Breed Not Found");

            }
            breed.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Breed Deleted Successfully");
        }
    }
}
