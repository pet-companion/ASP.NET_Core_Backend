using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            return new APIResponse<List<BreedVM>>(true, "All Breed Categories", breedVM, breedVM.Count());
        }

        public async Task<APIResponse<BreedVM>> GetBreed(int breedId)
        {
            var breed = await _dbContext.Breeds
                .FindAsync(breedId);
            if (breed is null)
            {
                return new APIResponse<BreedVM>(false, "Breed Not Found", null, 0);
            }
            var breedVM = _Mapper.Map<BreedVM>(breed);
            return new APIResponse<BreedVM>(true, "Breed Category", breedVM, 1);
        }

        public async Task<APIResponse> AddUpdateBreed(AddUpdaetBreedDto breedData)
        {
            var breed = new Breed();
            var msg = string.Empty;
            if (breedData is null || string.IsNullOrWhiteSpace(breedData.Name) || string.IsNullOrWhiteSpace(breedData.Description))
            {
                return new APIResponse(false, "All Fields Are Required");
            }
            //Edit
            if (breedData.Id != null)
            {
                breed = await _dbContext.Breeds.FindAsync(breedData.Id);
                if (breed is null)
                {
                    return new APIResponse(false, "Breed Not Found");
                }
                breed.LastUpdatedAt = DateTime.Now;
                msg = "Breed Updated Successfully";
            }
            breed.Name = breedData.Name;
            breed.Description = breedData.Description;
            if (breedData.Id == null)
            {
                await _dbContext.Breeds.AddAsync(breed);
                msg = "Breed Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeleteBreed(int breedId)
        {
            var breed = await _dbContext.Breeds
                .FindAsync(breedId);
            if (breed is null)
            {
                return new APIResponse(false, "Breed Not Found");
            }
            breed.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Breed Deleted Successfully");
        }
    }
}
