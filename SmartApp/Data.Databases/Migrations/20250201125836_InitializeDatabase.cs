using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAccountTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "longtext", nullable: false),
                    ImapServer = table.Column<string>(type: "longtext", nullable: false),
                    ImapPort = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    ConnectionTestPassed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccountTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAddressTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmailAddress = table.Column<string>(type: "longtext", nullable: false),
                    Domain = table.Column<string>(type: "longtext", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddressTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailDataTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FromAddress = table.Column<string>(type: "longtext", nullable: false),
                    Subject = table.Column<string>(type: "longtext", nullable: false),
                    Body = table.Column<string>(type: "longtext", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailDataTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailSubjectTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmailSubject = table.Column<string>(type: "longtext", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSubjectTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailTargetFolderTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ResourceKey = table.Column<string>(type: "longtext", nullable: false),
                    TargetFolderName = table.Column<string>(type: "longtext", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTargetFolderTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GenericSettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModuleName = table.Column<string>(type: "longtext", nullable: false),
                    ModuleType = table.Column<int>(type: "int", nullable: false),
                    SettingsJson = table.Column<string>(type: "longtext", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericSettingsTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogMessageTable",
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
                    table.PrimaryKey("PK_LogMessageTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailCleanerSettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmailCleanerEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCleanerSettingsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailCleanerSettingsTable_EmailAccountTable_AccountId",
                        column: x => x.AccountId,
                        principalTable: "EmailAccountTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "EmailTargetFolderTable",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ResourceKey", "TargetFolderName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4458), "System", "labelFolderUnknown", "Unknown", null, null },
                    { 2, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4461), "System", "labelFolderFoodOrder", "Food", null, null },
                    { 3, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4462), "System", "labelFolderTravel", "Travel", null, null },
                    { 4, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4463), "System", "labelFolderTax", "Tax", null, null },
                    { 5, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4463), "System", "labelFolderAccounts", "Accounts", null, null },
                    { 6, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4464), "System", "labelFolderHealth", "Health", null, null },
                    { 7, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4561), "System", "labelFolderRentAndReside", "RentAndReside", null, null },
                    { 8, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4564), "System", "labelFolderArchiv", "Archiv", null, null },
                    { 9, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4565), "System", "labelFolderSpam", "Spam", null, null },
                    { 10, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4567), "System", "labelFolderFamilyAndFriends", "FamilyAndFriends", null, null },
                    { 11, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4568), "System", "labelFolderShopping", "Shopping", null, null },
                    { 12, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4569), "System", "labelFolderSocialMedia", "SocialMedia", null, null },
                    { 13, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4569), "System", "labelFolderCar", "Car", null, null },
                    { 14, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4570), "System", "labelFolderTelecommunication", "Telecommunication", null, null },
                    { 15, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4571), "System", "labelFolderBankAndPayments", "BankAndPayments", null, null },
                    { 16, new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4571), "System", "labelFolderOther", "Other", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanerSettingsTable_AccountId",
                table: "EmailCleanerSettingsTable",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAddressTable");

            migrationBuilder.DropTable(
                name: "EmailCleanerSettingsTable");

            migrationBuilder.DropTable(
                name: "EmailDataTable");

            migrationBuilder.DropTable(
                name: "EmailSubjectTable");

            migrationBuilder.DropTable(
                name: "EmailTargetFolderTable");

            migrationBuilder.DropTable(
                name: "GenericSettingsTable");

            migrationBuilder.DropTable(
                name: "LogMessageTable");

            migrationBuilder.DropTable(
                name: "EmailAccountTable");
        }
    }
}
