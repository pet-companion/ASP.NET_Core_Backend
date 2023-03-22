using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IStoreService
    {
        Task<APIResponse<List<StoreVM>>> GetStoreList();
        Task<APIResponse<StoreVM>> GetStore(int storeId);
        Task<APIResponse> AddUpdateStore(AddUpdateStoreDto storeData);
        Task<APIResponse> DeleteStore(int storeId);
    }
}
