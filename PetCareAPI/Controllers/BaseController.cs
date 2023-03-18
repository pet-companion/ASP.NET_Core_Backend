using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetCareCore.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : Controller
    {
        protected int? _UserId;
        protected string _UserRole;
        protected string _UserEmail;
        public readonly IHttpContextAccessor _httpContext;

        public BaseController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            if (_httpContext != null && _httpContext.HttpContext.User != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _UserId = _httpContext.HttpContext.User.FindFirst(UserClaims.UserId) is null ? null : int.Parse(_httpContext.HttpContext.User.FindFirst(UserClaims.UserId).Value);
                _UserRole = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                _UserEmail = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            }
        }
    }
}
