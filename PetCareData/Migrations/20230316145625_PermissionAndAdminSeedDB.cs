using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCareData.Migrations
{
    public partial class PermissionAndAdminSeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastUpdatedAt", "Name" },
                values: new object[] { 1, new DateTime(2023, 3, 16, 16, 56, 25, 426, DateTimeKind.Local).AddTicks(7949), false, null, "Admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastUpdatedAt", "Name" },
                values: new object[] { 2, new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(5956), false, null, "Pet Owner" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastUpdatedAt", "Name" },
                values: new object[] { 3, new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(5973), false, null, "Store Owner" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "CreatedAt", "Email", "FullName", "ImgName", "IsBlocked", "IsDeleted", "IsEmailVerified", "LastUpdatedAt", "Password", "PhoneNumber", "RoleId", "RoleName" },
                values: new object[] { 1, null, null, new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(6122), "PetCareAdmin@gmail.com", "Admin", null, false, false, true, null, "Aa123$$", null, 1, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
