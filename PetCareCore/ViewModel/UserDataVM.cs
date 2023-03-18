using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class UserDataVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string RoleName { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsBlocked { get; set; }
        public AccessTokenViewModel Token { get; set; }
    }

    public class AccessTokenViewModel
    {
        public string BearerToken { get; set; }
        public DateTime ExpiringDate { get; set; }
    }
}
