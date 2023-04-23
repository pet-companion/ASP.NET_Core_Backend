using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCareCore.Constant;
using PetCareCore.Dto;
using PetCareCore.Enum;
using PetCareCore.ViewModel;
using PetCareData.Data;
using PetCareData.Models;
using PetCareInfrastructure.Helpers;
using PetCareInfrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly PetCareDbContext _dbContext;
        private readonly IMapper _Mapper;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public AuthenticationService(PetCareDbContext dbContext, IMapper mapper, IFileService fileService, IEmailService emailService)
        {
            _dbContext = dbContext;
            _Mapper = mapper;
            _fileService = fileService;
            _emailService = emailService;
        }

        public async Task<APIResponse> Register(AddNewUserDto userData)
        {
            try
            {
                if (userData is null || string.IsNullOrWhiteSpace(userData.Email) || string.IsNullOrWhiteSpace(userData.Password) || string.IsNullOrWhiteSpace(userData.RoleName) || userData.RoleId == 0)
                {
                    return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
                }
                //check Is Valid Email
                if (!StaticFunctionHelper.IsValidEmail(userData.Email))
                {
                    return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please enter valid email");
                }
                //check Is Strong Password
                if (!StaticFunctionHelper.IsStrongPassword(userData.Password))
                {
                    return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please enter strong password as 'MyPassword123!'");
                }
                var userImg = await _fileService.SaveFile(userData.UserImage, FileFolder.FolderName);
                var user = _Mapper.Map<User>(userData);
                user.IsEmailVerified = true;
                user.ImgName = userImg;
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "User Added Successfully");
            }
            catch (Exception ex)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.InternalServerError, "Something went wrong when adding the user", ex.Message);
            }
        }

        public async Task<APIResponse<UserDataVM>> Login(CredentialDataDto credentialData)
        {
            try
            {
                if (credentialData is null || credentialData.Email is null || credentialData.Password is null)
                {
                    return new APIResponse<UserDataVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required", null);
                }
                //check Is Valid Email
                if (!StaticFunctionHelper.IsValidEmail(credentialData.Email))
                {
                    return new APIResponse<UserDataVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please enter valid email", null);
                }
                var user = await _dbContext.Users
                    .SingleOrDefaultAsync(x => x.Email == credentialData.Email && x.Password == credentialData.Password);
                if (user is null)
                {
                    return new APIResponse<UserDataVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Invalid UserName Or Password", null);
                }
                if (!user.IsEmailVerified)
                {
                    return new APIResponse<UserDataVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Please Confirm Your Email Address", null);
                }
                var userData = _Mapper.Map<UserDataVM>(user);
                userData.Token = await GenerateAccessToken(user);
                //get user image
                //userData.UserImage = await _fileService.GetFile(user.ImgName, FileFolder.FolderName);
                return new APIResponse<UserDataVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "User Date", 1, userData);
            }
            catch (Exception ex)
            {
                return new APIResponse<UserDataVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.InternalServerError, "Something went wrong", ex.Message, null);
            }
        }

        public async Task<APIResponse<ForgotPasswordVM>> ForgotPassword(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return new APIResponse<ForgotPasswordVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "User Email Is Required", null);
            }
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == userEmail);
            if (user is null)
            {
                return new APIResponse<ForgotPasswordVM>(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "Email Not Found", null);
            }
            var randomNumber = StaticFunctionHelper.GnenrateRandomNumber();
            user.VerifiedCode = randomNumber;
            await _dbContext.SaveChangesAsync();
            //send email to user contains verified code
            await _emailService.SendEmail("ahmadalmikkawi123@gmail.com", "Verified Code", $"Verified Code Is: {randomNumber}");            
            return new APIResponse<ForgotPasswordVM>(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Please Enter Your Verification Code To Change Password", 1, new ForgotPasswordVM { UserEmail = userEmail, VerifiedCode = randomNumber });
        }

        public async Task<APIResponse> ChangePassword(ForgotPasswordDto userData)
        {
            if (userData is null || string.IsNullOrWhiteSpace(userData.UserEmail) || string.IsNullOrWhiteSpace(userData.NewPassword) || string.IsNullOrWhiteSpace(userData.VerifiedCode))
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "All Fields Are Required");
            }
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == userData.UserEmail && x.VerifiedCode == userData.VerifiedCode);
            if (user is null)
            {
                return new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.BadRequest, "The verification Code Or Email Is Incorrect");
            }
            user.Password = userData.NewPassword;
            await _dbContext.SaveChangesAsync();
            return new APIResponse(StatusMessageEnum.Success.ToDisplayName(), (int)HttpStatusCode.OK, "Password Changed successfully");
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
            var accessToken = new JwtSecurityToken(/*"https://localhost:44317/"*/ "http://petcompanion-001-site1.atempurl.com/",
                /*"https://localhost:44317/"*/ "http://petcompanion-001-site1.atempurl.com/",
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
