using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: true),
                    LastName = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationRequests", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 11, 51, 2, 816, DateTimeKind.Local).AddTicks(8526), new DateTime(2025, 3, 8, 11, 51, 2, 816, DateTimeKind.Local).AddTicks(8526), "U3VwZXJTZWNyZXRiOGRhMDlmNy1mM2MwLTQzMmUtODVmNC1mOWM3ZWY1OTU0MWI=", "b8da09f7-f3c0-432e-85f4-f9c7ef59541b", new DateTime(2024, 12, 8, 11, 51, 2, 816, DateTimeKind.Local).AddTicks(8526) });

            migrationBuilder.UpdateData(
                table: "UserModules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 11, 51, 2, 817, DateTimeKind.Local).AddTicks(170), new DateTime(2024, 12, 8, 11, 51, 2, 817, DateTimeKind.Local).AddTicks(170) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 11, 51, 2, 816, DateTimeKind.Local).AddTicks(8864), new DateTime(2024, 12, 8, 11, 51, 2, 816, DateTimeKind.Local).AddTicks(8864) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationRequests");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116), new DateTime(2025, 3, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116), "U3VwZXJTZWNyZXQ3MzMzYmU4YS1kZTc4LTQ5M2QtOGRhOS05ZWI5NTgyYmVjZWM=", "7333be8a-de78-493d-8da9-9eb9582becec", new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6116) });

            migrationBuilder.UpdateData(
                table: "UserModules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(7971), new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(7971) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6511), new DateTime(2024, 12, 8, 10, 57, 37, 112, DateTimeKind.Local).AddTicks(6511) });
        }
    }
}
