using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            Sql.SeedModules(migrationBuilder.Sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 40, 58, 839, DateTimeKind.Local).AddTicks(9894), new DateTime(2025, 3, 8, 10, 40, 58, 839, DateTimeKind.Local).AddTicks(9894), "U3VwZXJTZWNyZXRmNjhjNGQ2NS00MzNiLTQ2OWItOTQ3Yy04ZjZiZGRlNzM4ZWM=", "f68c4d65-433b-469b-947c-8f6bdde738ec", new DateTime(2024, 12, 8, 10, 40, 58, 839, DateTimeKind.Local).AddTicks(9894) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 40, 58, 840, DateTimeKind.Local).AddTicks(239), new DateTime(2024, 12, 8, 10, 40, 58, 840, DateTimeKind.Local).AddTicks(239) });
        }
    }
}
