using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCareCore.Constant;
using PetCareCore.Dto;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Helpers;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class Authentication : IAuthentication
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;

        public Authentication(PetCareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
        }

        public async Task<APIResponse> Register(AddNewUserDto userData)
        {
            try
            {
                if (userData is null || string.IsNullOrWhiteSpace(userData.Email) || string.IsNullOrWhiteSpace(userData.Password) || string.IsNullOrWhiteSpace(userData.RoleName) || userData.RoleId == 0)
                {
                    return new APIResponse(false, "All Fields Are Required");
                }
                var user = _Mapper.Map<User>(userData);
                user.IsEmailVerified = true;
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return new APIResponse(true, "User Added Successfully");
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "Something went wrong When Add User", new() { ex.Message });
            }
        }

        public async Task<APIResponse<UserDataVM>> Login(CredentialDataDto credentialData)
        {
            if (credentialData is null || credentialData.Email is null || credentialData.Password is null)
            {
                return new APIResponse<UserDataVM>(false, "All Fields Are Required", null);
            }
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == credentialData.Email && x.Password == credentialData.Password);
            if (user is null)
            {
                return new APIResponse<UserDataVM>(false, "Invalid UserName Or Password", null);
            }
            if (!user.IsEmailVerified)
            {
                return new APIResponse<UserDataVM>(false, "Please Confirm Your Email Address", null);
            }
            var userData = _Mapper.Map<UserDataVM>(user);
            userData.Token = await GenerateAccessToken(user);
            return new APIResponse<UserDataVM>(true, "Success", userData, 1);
        }

        public async Task<APIResponse<ForgotPasswordVM>> ForgotPassword(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return new APIResponse<ForgotPasswordVM>(false, "User Email Is Required", null);
            }
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == userEmail);
            if (user is null)
            {
                return new APIResponse<ForgotPasswordVM>(false, "Email Not Found", null);
            }
            var randomNumber = StaticFunctionHelper.GnenrateRandomNumber();
            user.VerifiedCode = randomNumber;
            await _dbContext.SaveChangesAsync();
            return new APIResponse<ForgotPasswordVM>(true, "Please Enter Your Verification Code To Change Password", new ForgotPasswordVM { UserEmail = userEmail, VerifiedCode = randomNumber }, 1);
        }

        public async Task<APIResponse> ChangePassword(ForgotPasswordDto userData)
        {
            if (userData is null || string.IsNullOrWhiteSpace(userData.UserEmail) || string.IsNullOrWhiteSpace(userData.NewPassword) || string.IsNullOrWhiteSpace(userData.VerifiedCode))
            {
                return new APIResponse(false, "All Fields Are Required", null);
            }
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == userData.UserEmail && x.VerifiedCode == userData.VerifiedCode);
            if (user is null)
            {
                return new APIResponse(false, "The verification Code Or Email Is Incorrect", null);
            }
            user.Password = userData.NewPassword;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(true, "Password Changed successfully", null);
        }

        private async Task<AccessTokenViewModel> GenerateAccessToken(User user)
        {
            // Create User Informations(Claims)
            var claims = new List<Claim>(){
            new Claim(ClaimTypes.Role, user.RoleName),//To Know User Role In Authorize
            new Claim(ClaimTypes.Email, user.Email),//To Know User Email In Authorize
            new Claim(UserClaims.UserId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("djksjkccyjkdvujkksasjscyddnagwui"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(30);
            var accessToken = new JwtSecurityToken("https://localhost:44317/",
                "https://localhost:44317/",
            claims,
            expires: expires,
            signingCredentials: credentials
            );
            string AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
            return new AccessTokenViewModel()
            {
                BearerToken = AccessToken,
                ExpiringDate = expires
            };
        }
    }
}
