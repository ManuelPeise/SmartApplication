using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddUseScheduledEmailDataImportToEmailCleanerSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseScheduledEmailDataExport",
                table: "EmailCleanerSettingsTable",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1634));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1637));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1638));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1639));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1640));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1641));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1642));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1643));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1643));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1647));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1647));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1648));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1648));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1649));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1649));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 18, 22, 29, 217, DateTimeKind.Utc).AddTicks(1650));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseScheduledEmailDataExport",
                table: "EmailCleanerSettingsTable");

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(786));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(791));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(792));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(792));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(793));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(817));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(817));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(818));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(819));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(820));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(820));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(821));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(822));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(822));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(823));

            migrationBuilder.UpdateData(
                table: "EmailTargetFolderTable",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 1, 14, 28, 37, 139, DateTimeKind.Utc).AddTicks(823));
        }
    }
}
