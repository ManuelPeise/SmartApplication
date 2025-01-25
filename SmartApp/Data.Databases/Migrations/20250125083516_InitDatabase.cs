using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Databases.Migrations
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
                    Group = table.Column<string>(type: "longtext", nullable: false),
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
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ResourceKey = table.Column<string>(type: "longtext", nullable: false),
                    RoleName = table.Column<string>(type: "longtext", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    IsNewUserRegistration = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CredentialsId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Credentials_CredentialsId",
                        column: x => x.CredentialsId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            SqlScripts.IdentitySql.SeedUserRoles(migrationBuilder.Sql);

            migrationBuilder.InsertData(
                table: "AccessRights",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Group", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3110), "System", "Administration", "UserAdministration", null, null },
                    { 2, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3115), "System", "Administration", "SpamPredictionSettings", null, null },
                    { 3, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3117), "System", "Administration", "MessageLog", null, null },
                    { 4, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3119), "System", "Interface", "EmailAccountInterface", null, null },
                    { 5, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3121), "System", "Tools", "EmailCleaner", null, null },
                    { 6, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3133), "System", "Settings", "EmailAccountSettings", null, null },
                    { 7, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(3136), "System", "Settings", "EmailCleanerSettings", null, null }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ExpiresAt", "Password", "RefreshToken", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 25, 9, 35, 16, 508, DateTimeKind.Local).AddTicks(3222), "System", new DateTime(2025, 4, 25, 9, 35, 16, 508, DateTimeKind.Local).AddTicks(3222), "Am1I3JdgO3aS/VUSZ8kfKQ==", "", new DateTime(2025, 1, 25, 9, 35, 16, 508, DateTimeKind.Local).AddTicks(3222), "System" });

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5076), "System", false, true, null, null, 1, true },
                    { 2, 2, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5079), "System", false, true, null, null, 1, true },
                    { 3, 3, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5080), "System", false, true, null, null, 1, true },
                    { 4, 4, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5081), "System", false, true, null, null, 1, true },
                    { 5, 5, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5082), "System", false, true, null, null, 1, true },
                    { 6, 6, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5085), "System", false, true, null, null, 1, true },
                    { 7, 7, new DateTime(2025, 1, 25, 8, 35, 16, 508, DateTimeKind.Utc).AddTicks(5088), "System", false, true, null, null, 1, true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CredentialsId", "Email", "FirstName", "IsActive", "IsNewUserRegistration", "LastName", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 25, 9, 35, 16, 508, DateTimeKind.Local).AddTicks(4958), "System", 1, "admin.user@gmx.de", "Admin", true, false, "User", 2, new DateTime(2025, 1, 25, 9, 35, 16, 508, DateTimeKind.Local).AddTicks(4958), "System" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessRights_AccessRightId",
                table: "UserAccessRights",
                column: "AccessRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CredentialsId",
                table: "Users",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccessRights");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AccessRights");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
