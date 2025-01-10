using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

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
                name: "EmailCleanerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Account = table.Column<string>(type: "longtext", nullable: false),
                    AccountName = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AllowReadEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowMoveEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowDeleteEmails = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowCreateEmailFolder = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShareEmailDataToTrainAi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScheduleCleanup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScheduleCleanupAtHour = table.Column<int>(type: "int", nullable: false),
                    LastCleanupTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NextCleanupTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                    EmailCleanerSettingsId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAccounts_EmailCleanerSettings_EmailCleanerSettingsId",
                        column: x => x.EmailCleanerSettingsId,
                        principalTable: "EmailCleanerSettings",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAddressMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SourceAddress = table.Column<string>(type: "longtext", nullable: true),
                    Domain = table.Column<string>(type: "longtext", nullable: true),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAccounts");

            migrationBuilder.DropTable(
                name: "EmailAddressMappings");

            migrationBuilder.DropTable(
                name: "LogMessages");

            migrationBuilder.DropTable(
                name: "EmailCleanerSettings");
        }
    }
}
