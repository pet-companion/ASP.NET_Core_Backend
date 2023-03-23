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
            return new APIResponse<List<StoreVM>>(true, "All Stores Data", storeVM, storeVM.Count());
        }

        public async Task<APIResponse<StoreVM>> GetStore(int storeId)
        {
            var store = await _dbContext.Stores
                .FindAsync(storeId);
            if (store is null)
            {
                return new APIResponse<StoreVM>(false, "Store Not Found", null, 0);
            }
            var storeVM = _Mapper.Map<StoreVM>(store);
            return new APIResponse<StoreVM>(true, "Store Data", storeVM, 1);
        }

        public async Task<APIResponse> AddUpdateStore(AddUpdateStoreDto storeData)
        {
            var store = new Store();
            var msg = string.Empty;
            if (storeData is null || string.IsNullOrWhiteSpace(storeData.Name) || string.IsNullOrWhiteSpace(storeData.Address) || string.IsNullOrWhiteSpace(storeData.PhoneNumber) || string.IsNullOrWhiteSpace(storeData.Email))
            {
                return new APIResponse(false, "All Fields Are Required"); 
            }
            var user = _dbContext.Users.Find(storeData.UserId);
            if (user is null)
            {
                return new APIResponse(false, "User Not Found");
            }
            //Edit
            if (storeData.Id != null)
            {
                store = await _dbContext.Stores.FindAsync(storeData.Id);
                if (store is null)
                {
                    return new APIResponse(false, "Store Not Found");
                }
                store.LastUpdatedAt = DateTime.Now;
                msg = "Store Updated Successfully";
            }
            store.Name = storeData.Name;
            store.Address = storeData.Address;
            store.PhoneNumber = storeData.PhoneNumber;
            store.Email = storeData.Email;
            store.UserId = storeData.UserId;
            if (storeData.Id == null)
            {
                await _dbContext.Stores.AddAsync(store);
                msg = "Store Added Successfully";
            }
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, msg);
        }

        public async Task<APIResponse> DeleteStore(int storeId)
        {
            var store = await _dbContext.Stores
                .FindAsync(storeId);
            if (store is null)
            {
                return new APIResponse(false, "Store Not Found");
            }
            store.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Store Deleted Successfully");
        }
    }
}
