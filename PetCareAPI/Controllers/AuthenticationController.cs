using Microsoft.AspNetCore.Authorization;
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
    public class AuthenticationController : BaseController
    {
        private readonly IAuthentication _authentication;

        public AuthenticationController(IAuthentication authentication, IHttpContextAccessor httpContext):base(httpContext)
        {
            _authentication = authentication;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] AddNewUserDto userData)
        {
            var res = await _authentication.Register(userData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialDataDto credentialData)
        {
            var res = await _authentication.Login(credentialData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            var res = await _authentication.ForgotPassword(userEmail);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ForgotPasswordDto userData)
        {
            var res = await _authentication.ChangePassword(userData);
            if (res.status)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
