using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IPetService
    {
        Task<APIResponse<List<PetVM>>> GetPetList();
        Task<APIResponse<PetVM>> GetPet(int petId);
        Task<APIResponse> AddUpdatePet(AddUpdatePetDto petData);
        Task<APIResponse> DeletePet(int petId);
    }
}
