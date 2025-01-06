using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Sql.SeedUserRoles(migrationBuilder.Sql);

            migrationBuilder.InsertData(
                table: "AccessRights",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Group", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(6118), "System", "Administration", "UserAdministration", null, null },
                    { 2, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(6124), "System", "Administration", "MessageLog", null, null },
                    { 3, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(6126), "System", "Settings", "EmailAccountSettings", null, null }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ExpiresAt", "Password", "RefreshToken", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 6, 9, 24, 9, 671, DateTimeKind.Local).AddTicks(6209), "System", new DateTime(2025, 4, 6, 9, 24, 9, 671, DateTimeKind.Local).AddTicks(6209), "Am1I3JdgO3aS/VUSZ8kfKQ==", "", new DateTime(2025, 1, 6, 9, 24, 9, 671, DateTimeKind.Local).AddTicks(6209), "System" });

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(8228), "System", false, true, null, null, 1, true },
                    { 2, 2, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(8234), "System", false, true, null, null, 1, true },
                    { 3, 3, new DateTime(2025, 1, 6, 8, 24, 9, 671, DateTimeKind.Utc).AddTicks(8236), "System", false, true, null, null, 1, true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CredentialsId", "Email", "FirstName", "IsActive", "LastName", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 6, 9, 24, 9, 671, DateTimeKind.Local).AddTicks(8150), "System", 1, "admin.user@gmx.de", "Admin", true, "User", 2, new DateTime(2025, 1, 6, 9, 24, 9, 671, DateTimeKind.Local).AddTicks(8150), "System" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
