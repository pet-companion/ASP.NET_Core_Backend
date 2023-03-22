using PetCareCore.Dto;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<APIResponse> Register(AddNewUserDto userData);
        Task<APIResponse<UserDataVM>> Login(CredentialDataDto credentialData);
        Task<APIResponse<ForgotPasswordVM>> ForgotPassword(string userEmail);
        Task<APIResponse> ChangePassword(ForgotPasswordDto userData);
    }
}
