using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Enum
{
    public enum RoleEnum
    {
        Admin = 1,
        PetOwner = 2,
        StoreOwner = 3,
    }

    public static class RoleExtensions
    {
        public static string ToDisplayName(this RoleEnum RoleEnum)
        {
            return RoleEnum switch
            {
                RoleEnum.Admin => "Admin",
                RoleEnum.PetOwner => "Pet Owner",
                RoleEnum.StoreOwner => "Store Owner",
                _ => "Unknown Role",
            };
        }
    }
}
