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
    public class PetController : BaseController
    {
        private readonly IPetService _petService;

        public PetController(IHttpContextAccessor httpContext, IPetService petService) : base(httpContext)
        {
            _petService = petService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPetList()
        {
            var res = await _petService.GetPetList();
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetPet(int petId)
        {
            var res = await _petService.GetPet(petId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddPet([FromForm] AddUpdatePetDto petData)
        {
            var res = await _petService.AddUpdatePet(petData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePet([FromForm] AddUpdatePetDto petData)
        {
            var res = await _petService.AddUpdatePet(petData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePet(int petId)
        {
            var res = await _petService.DeletePet(petId);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
