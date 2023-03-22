using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareCore.Dto;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetCareAPI.Controllers
{
    public class StoreController : BaseController
    {
        private readonly IStoreService _storeService;

        public StoreController(IHttpContextAccessor httpContext, IStoreService storeService) : base(httpContext)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStoreList()
        {
            var res = await _storeService.GetStoreList();
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetStore(int storeId)
        {
            var res = await _storeService.GetStore(storeId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateStore([FromBody] AddUpdateStoreDto storeData)
        {
            var res = await _storeService.AddUpdateStore(storeData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            var res = await _storeService.DeleteStore(storeId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
