using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116), new DateTime(2025, 3, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116), "U3VwZXJTZWNyZXQ3MzMzYmU4YS1kZTc4LTQ5M2QtOGRhOS05ZWI5NTgyYmVjZWM=", "7333be8a-de78-493d-8da9-9eb9582becec", new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116) });

            migrationBuilder.InsertData(
                table: "UserModules",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Deny", "HasReadAccess", "HasWriteAccess", "ModuleId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { 1, new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(7971), "System", false, true, true, 1, new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(7971), "System", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6511), new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6511) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserModules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 43, 46, 864, DateTimeKind.Local).AddTicks(143), new DateTime(2025, 3, 8, 10, 43, 46, 864, DateTimeKind.Local).AddTicks(143), "U3VwZXJTZWNyZXRhODBhYjgxMC0zOTBiLTQ3ZDMtYTU4MS1kOTQzNzBhMDRiZGM=", "a80ab810-390b-47d3-a581-d94370a04bdc", new DateTime(2024, 12, 8, 10, 43, 46, 864, DateTimeKind.Local).AddTicks(143) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 43, 46, 864, DateTimeKind.Local).AddTicks(477), new DateTime(2024, 12, 8, 10, 43, 46, 864, DateTimeKind.Local).AddTicks(477) });
        }
    }
}
