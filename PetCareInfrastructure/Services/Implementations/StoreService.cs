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
    public class StoreService : IStoreService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;

        public StoreService(PetCareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
        }

        public async Task<APIResponse<List<StoreVM>>> GetStoreList()
        {
            var storeList = await _dbContext.Stores.ToListAsync();
            var storeVM = _Mapper.Map<List<StoreVM>>(storeList);
            return new APIResponse<List<StoreVM>>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Stores List", storeVM.Count(), storeVM);
        }

        public async Task<APIResponse<StoreVM>> GetStore(int storeId)
        {
            var store = await _dbContext.Stores
                .FindAsync(storeId);
            if (store is null)
            {
                return new APIResponse<StoreVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Store Not Found", null);
            }
            var storeVM = _Mapper.Map<StoreVM>(store);
            return new APIResponse<StoreVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Store Data", 1, storeVM);
        }

        public async Task<APIResponse> AddStore(AddStoreDto storeData)
        {
            if (storeData is null || string.IsNullOrWhiteSpace(storeData.Name) || string.IsNullOrWhiteSpace(storeData.Address) || string.IsNullOrWhiteSpace(storeData.PhoneNumber) || string.IsNullOrWhiteSpace(storeData.Email))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            var user = _dbContext.Users.Find(storeData.UserId);
            if (user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Not Found");
            }
            var store = _Mapper.Map<Store>(storeData);
            await _dbContext.Stores.AddAsync(store);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Store Added Successfully");
        }

        public async Task<APIResponse> UpdateStore(UpdateStoreDto storeData)
        {
            if (storeData is null || storeData.Id is null || string.IsNullOrWhiteSpace(storeData.Name) || string.IsNullOrWhiteSpace(storeData.Address) || string.IsNullOrWhiteSpace(storeData.PhoneNumber) || string.IsNullOrWhiteSpace(storeData.Email))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            var user = _dbContext.Users.Find(storeData.UserId);
            if (user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Not Found");
            }
            var dbStore = await _dbContext.Stores.FindAsync(storeData.Id);
            if (dbStore is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Store Not Found");
            }
             _Mapper.Map(storeData, dbStore);
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Store Updated Successfully");
        }

        public async Task<APIResponse> DeleteStore(int storeId)
        {
            var store = await _dbContext.Stores
                .FindAsync(storeId);
            if (store is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Store Not Found");
            }
            store.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Store Deleted Successfully");
        }
    }
}
