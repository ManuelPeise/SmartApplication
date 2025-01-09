using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.AppContext.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailCleanerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailCleanerSettingsId",
                table: "EmailAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailCleanerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowReadEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowMoveEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowDeleteEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowCreateEmailFolder = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowUseEmailDataForSpamDetectionAiTraining = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScheduledCleanupAtHour = table.Column<int>(type: "int", nullable: false),
                    LastCleanupTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextCleanupTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCleanerSettings", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAddressMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SourceAddress = table.Column<string>(type: "longtext", nullable: false),
                    Domain = table.Column<string>(type: "longtext", nullable: false),
                    ShouldCleanup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSpam = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PredictedAs = table.Column<string>(type: "longtext", nullable: true),
                    EmailCleanerSettingsId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddressMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddressMappings_EmailCleanerSettings_EmailCleanerSettin~",
                        column: x => x.EmailCleanerSettingsId,
                        principalTable: "EmailCleanerSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAccounts_EmailCleanerSettingsId",
                table: "EmailAccounts",
                column: "EmailCleanerSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddressMappings_EmailCleanerSettingsId",
                table: "EmailAddressMappings",
                column: "EmailCleanerSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_EmailCleanerSettings_EmailCleanerSettingsId",
                table: "EmailAccounts",
                column: "EmailCleanerSettingsId",
                principalTable: "EmailCleanerSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_EmailCleanerSettings_EmailCleanerSettingsId",
                table: "EmailAccounts");

            migrationBuilder.DropTable(
                name: "EmailAddressMappings");

            migrationBuilder.DropTable(
                name: "EmailCleanerSettings");

            migrationBuilder.DropIndex(
                name: "IX_EmailAccounts_EmailCleanerSettingsId",
                table: "EmailAccounts");

            migrationBuilder.DropColumn(
                name: "EmailCleanerSettingsId",
                table: "EmailAccounts");
        }
    }
}
