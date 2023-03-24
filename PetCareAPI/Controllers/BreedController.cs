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
    public class BreedController : BaseController
    {
        private readonly IBreedService _breedService;

        public BreedController(IHttpContextAccessor httpContext, IBreedService breedService) : base(httpContext)
        {
            _breedService = breedService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBreedList()
        {
            var res = await _breedService.GetBreedList();
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetBreed(int breedId)
        {
            var res = await _breedService.GetBreed(breedId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddBreed([FromBody] AddUpdaetBreedDto breedData)
        {
            var res = await _breedService.AddUpdateBreed(breedData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBreed([FromBody] AddUpdaetBreedDto breedData)
        {
            var res = await _breedService.AddUpdateBreed(breedData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBreed(int breedId)
        {
            var res = await _breedService.DeleteBreed(breedId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
