using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IBreedService
    {
        Task<APIResponse<List<BreedVM>>> GetBreedList();
        Task<APIResponse<BreedVM>> GetBreed(int breedId);
        Task<APIResponse> AddUpdateBreed(AddUpdaetBreedDto breedData);
        Task<APIResponse> DeleteBreed(int breedId);
    }
}
