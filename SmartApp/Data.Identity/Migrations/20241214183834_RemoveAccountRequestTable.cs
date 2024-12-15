using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationRequests");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(7462), new DateTime(2025, 3, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(7462), "U3VwZXJTZWNyZXQxMWJlYTFhZS0yMTJhLTQwM2ItODg4OS1jYzE1Y2Q3MGNiZGE=", "11bea1ae-212a-403b-8889-cc15cd70cbda", new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(7462) });

            migrationBuilder.UpdateData(
                table: "UserModules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(9235), new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(7908), new DateTime(2024, 12, 14, 19, 38, 32, 637, DateTimeKind.Local).AddTicks(7908) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    FirstName = table.Column<string>(type: "longtext", nullable: true),
                    LastName = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false)
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
    }
}
