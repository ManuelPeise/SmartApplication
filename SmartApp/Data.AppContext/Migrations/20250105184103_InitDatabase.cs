using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.AppContext.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessRights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRights", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "longtext", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<string>(type: "longtext", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "longtext", nullable: false),
                    EncodedPassword = table.Column<string>(type: "longtext", nullable: false),
                    MessageLogJson = table.Column<string>(type: "longtext", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccounts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "longtext", nullable: true),
                    Module = table.Column<string>(type: "longtext", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogMessages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAccessRights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessRightId = table.Column<int>(type: "int", nullable: false),
                    View = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Edit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deny = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessRights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccessRights_AccessRights_AccessRightId",
                        column: x => x.AccessRightId,
                        principalTable: "AccessRights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AccessRights",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(382), "System", "Administration", null, null },
                    { 2, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(389), "System", "UserAdministration", null, null },
                    { 3, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(390), "System", "Settings", null, null },
                    { 4, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(390), "System", "EmailAccountSettings", null, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(481), "System", false, true, null, null, 1, true },
                    { 2, 2, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(485), "System", false, true, null, null, 1, true },
                    { 3, 3, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(486), "System", false, true, null, null, 1, true },
                    { 4, 4, new DateTime(2025, 1, 5, 18, 41, 1, 265, DateTimeKind.Utc).AddTicks(487), "System", false, true, null, null, 1, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessRights_AccessRightId",
                table: "UserAccessRights",
                column: "AccessRightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAccounts");

            migrationBuilder.DropTable(
                name: "LogMessages");

            migrationBuilder.DropTable(
                name: "UserAccessRights");

            migrationBuilder.DropTable(
                name: "AccessRights");
        }
    }
}
