using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareCore.Dto;
using PetCareCore.Enum;
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
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetStore(int storeId)
        {
            var res = await _storeService.GetStore(storeId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddStore([FromBody] AddStoreDto storeData)
        {
            var res = await _storeService.AddStore(storeData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStore([FromBody] UpdateStoreDto storeData)
        {
            var res = await _storeService.UpdateStore(storeData);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            var res = await _storeService.DeleteStore(storeId);
            if (res.Status == StatusMessageEnum.Success.ToDisplayName())
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
