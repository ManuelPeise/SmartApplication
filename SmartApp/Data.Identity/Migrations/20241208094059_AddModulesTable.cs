using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddModulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ModuleName = table.Column<string>(type: "longtext", nullable: true),
                    ModuleType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Deny = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasReadAccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasWriteAccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserModules_ModuleId",
                table: "UserModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModules_UserId",
                table: "UserModules",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModules");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905), new DateTime(2025, 3, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905), "U3VwZXJTZWNyZXQwNGMxMDBiNS0yYzEzLTRjYzQtOWYzNS0xYjFlNjY1MGFmNGE=", "04c100b5-2c13-4cc4-9f35-1b1e6650af4a", new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(4905) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(5282), new DateTime(2024, 12, 8, 7, 47, 19, 545, DateTimeKind.Local).AddTicks(5282) });
        }
    }
}
