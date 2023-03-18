using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCareData.Migrations
{
    public partial class AddVerifiedCodeToTableUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifiedCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 18, 23, 24, 9, 895, DateTimeKind.Local).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 18, 23, 24, 9, 897, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 18, 23, 24, 9, 897, DateTimeKind.Local).AddTicks(2893));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RoleName" },
                values: new object[] { new DateTime(2023, 3, 18, 23, 24, 9, 897, DateTimeKind.Local).AddTicks(3045), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedCode",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 16, 16, 56, 25, 426, DateTimeKind.Local).AddTicks(7949));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(5956));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RoleName" },
                values: new object[] { new DateTime(2023, 3, 16, 16, 56, 25, 428, DateTimeKind.Local).AddTicks(6122), null });
        }
    }
}
