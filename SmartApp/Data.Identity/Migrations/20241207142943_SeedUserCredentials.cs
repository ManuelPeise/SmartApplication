using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ExpiresAt", "Password", "Salt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2024, 12, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269), "System", new DateTime(2025, 3, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269), "U3VwZXJTZWNyZXQzMGU5MmYzNy0wNjFkLTRiZGQtYTAxMy1mMTNiOWU1NDYyM2Y=", "30e92f37-061d-4bdd-a013-f13b9e54623f", new DateTime(2024, 12, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269), "System" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
