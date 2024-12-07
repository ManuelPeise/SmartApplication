using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Credentials_CredentialsId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CredentialsId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753), new DateTime(2025, 3, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753), "U3VwZXJTZWNyZXQ1MzRhYTQ1Yi05YWRkLTQzNGUtYWQzYy1mNzJlNWFkNTA1ZGE=", "534aa45b-9add-434e-ad3c-f72e5ad505da", new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CredentialsId", "Email", "FirstName", "IsActive", "LastName", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(6078), "System", 1, "admin.user@gmx.de", "Admin", true, "User", 2, new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(6078), "System" });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Credentials_CredentialsId",
                table: "Users",
                column: "CredentialsId",
                principalTable: "Credentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Credentials_CredentialsId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CredentialsId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExpiresAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269), new DateTime(2025, 3, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269), "U3VwZXJTZWNyZXQzMGU5MmYzNy0wNjFkLTRiZGQtYTAxMy1mMTNiOWU1NDYyM2Y=", "30e92f37-061d-4bdd-a013-f13b9e54623f", new DateTime(2024, 12, 7, 15, 29, 43, 163, DateTimeKind.Local).AddTicks(8269) });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Credentials_CredentialsId",
                table: "Users",
                column: "CredentialsId",
                principalTable: "Credentials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id");
        }
    }
}
