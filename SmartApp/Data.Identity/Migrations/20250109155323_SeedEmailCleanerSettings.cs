using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmailCleanerSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(2108));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(2111));

            migrationBuilder.InsertData(
                table: "AccessRights",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Group", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 4, new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(2114), "System", "Settings", "EmailCleanerEmailCleanerSettings", null, null });

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 53, 22, 932, DateTimeKind.Local).AddTicks(2249), new DateTime(2025, 4, 9, 16, 53, 22, 932, DateTimeKind.Local).AddTicks(2249), new DateTime(2025, 1, 9, 16, 53, 22, 932, DateTimeKind.Local).AddTicks(2249) });

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(5254));

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(5259));

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(5261));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 9, 16, 53, 22, 932, DateTimeKind.Local).AddTicks(5135), new DateTime(2025, 1, 9, 16, 53, 22, 932, DateTimeKind.Local).AddTicks(5135) });

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[] { 4, 4, new DateTime(2025, 1, 9, 15, 53, 22, 932, DateTimeKind.Utc).AddTicks(5264), "System", false, true, null, null, 1, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(5189));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(5193));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(5195));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 8, 16, 28, 42, 901, DateTimeKind.Local).AddTicks(5267), new DateTime(2025, 4, 8, 16, 28, 42, 901, DateTimeKind.Local).AddTicks(5267), new DateTime(2025, 1, 8, 16, 28, 42, 901, DateTimeKind.Local).AddTicks(5267) });

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 8, 15, 28, 42, 901, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 8, 16, 28, 42, 901, DateTimeKind.Local).AddTicks(7201), new DateTime(2025, 1, 8, 16, 28, 42, 901, DateTimeKind.Local).AddTicks(7201) });
        }
    }
}
