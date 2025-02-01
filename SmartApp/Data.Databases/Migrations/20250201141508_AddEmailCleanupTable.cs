using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailCleanupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenericSettingsTable");

            migrationBuilder.CreateTable(
                name: "EmailCleanupTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TargetFolderId = table.Column<int>(type: "int", nullable: false),
                    PredictedTargetFolderId = table.Column<int>(type: "int", nullable: true),
                    SpamIdentifierValue = table.Column<int>(type: "int", nullable: false),
                    PredictedSpamIdentifierValue = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCleanupTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailCleanupTable_EmailAccountTable_AccountId",
                        column: x => x.AccountId,
                        principalTable: "EmailAccountTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailCleanupTable_EmailAddressTable_AddressId",
                        column: x => x.AddressId,
                        principalTable: "EmailAddressTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailCleanupTable_EmailSubjectTable_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "EmailSubjectTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailCleanupTable_EmailTargetFolderTable_PredictedTargetFold~",
                        column: x => x.PredictedTargetFolderId,
                        principalTable: "EmailTargetFolderTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailCleanupTable_EmailTargetFolderTable_TargetFolderId",
                        column: x => x.TargetFolderId,
                        principalTable: "EmailTargetFolderTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4314));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4318));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4318));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4321));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4322));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4322));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4323));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4324));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4324));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326));

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanupTable_AccountId",
                table: "EmailCleanupTable",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanupTable_AddressId",
                table: "EmailCleanupTable",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanupTable_PredictedTargetFolderId",
                table: "EmailCleanupTable",
                column: "PredictedTargetFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanupTable_SubjectId",
                table: "EmailCleanupTable",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCleanupTable_TargetFolderId",
                table: "EmailCleanupTable",
                column: "TargetFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailCleanupTable");

            migrationBuilder.CreateTable(
                name: "GenericSettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    ModuleName = table.Column<string>(type: "longtext", nullable: false),
                    ModuleType = table.Column<int>(type: "int", nullable: false),
                    SettingsJson = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericSettingsTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4458));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4461));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4463));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4463));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4561));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4565));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4569));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4569));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 12, 58, 34, 219, DateTimeKind.Utc).AddTicks(4571));
        }
    }
}
