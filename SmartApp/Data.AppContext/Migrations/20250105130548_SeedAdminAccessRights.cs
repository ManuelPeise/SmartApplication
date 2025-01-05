using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.AppContext.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAccessRights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4614));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4615));

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4740), "System", false, true, null, null, 1, true },
                    { 2, 2, new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4742), "System", false, true, null, null, 1, true },
                    { 3, 3, new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4743), "System", false, true, null, null, 1, true },
                    { 4, 4, new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4744), "System", false, true, null, null, 1, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserAccessRights",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 12, 2, 43, 181, DateTimeKind.Utc).AddTicks(6536));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 12, 2, 43, 181, DateTimeKind.Utc).AddTicks(6539));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 12, 2, 43, 181, DateTimeKind.Utc).AddTicks(6540));

            migrationBuilder.UpdateData(
                table: "AccessRights",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 12, 2, 43, 181, DateTimeKind.Utc).AddTicks(6540));
        }
    }
}
