using Microsoft.EntityFrameworkCore;
using PetCareCore.Enum;
using PetCareData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareData
{
    public static class UsersPermissionSeeder
    {
        public static void PermissionSeeder(this ModelBuilder modelBuilder)
        {
            var allPermissions = new List<Role>();
            var adminUser = new List<User>();
            allPermissions.Add(new Role
            {
                Id = 1,
                Name = "Admin"
            });
            allPermissions.Add(new Role
            {
                Id = 2,
                Name = "Pet Owner"
            });
            allPermissions.Add(new Role
            {
                Id = 3,
                Name = "Store Owner"
            });
            adminUser.Add(new User
            {
                Id = 1,
                Email = "PetCareAdmin@gmail.com",
                Password = "Aa123$$",
                FullName = "Admin",
                IsEmailVerified = true,
                RoleId = 1,
                RoleName = RoleEnum.Admin.ToDisplayName()
            });
            modelBuilder.Entity<Role>().HasData(allPermissions);
            modelBuilder.Entity<User>().HasData(adminUser);
        }
    }
}
