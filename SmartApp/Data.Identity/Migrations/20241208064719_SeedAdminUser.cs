using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ExpiresAt", "Password", "RefreshToken", "Salt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905), "System", new DateTime(2025, 3, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905), "U3VwZXJTZWNyZXQwNGMxMDBiNS0yYzEzLTRjYzQtOWYzNS0xYjFlNjY1MGFmNGE=", "", "04c100b5-2c13-4cc4-9f35-1b1e6650af4a", new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905), "System" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CredentialsId", "Email", "FirstName", "IsActive", "LastName", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(5282), "System", 1, "admin.user@gmx.de", "Admin", true, "User", 2, new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(5282), "System" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
