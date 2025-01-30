using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Databases.Migrations.UserIdentity
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
                    { 1, new DateTime(2025, 1, 30, 8, 18, 27, 146, DateTimeKind.Utc).AddTicks(5319), "System", "Administration", "UserAdministration", null, null },
                    { 2, new DateTime(2025, 1, 30, 8, 18, 27, 146, DateTimeKind.Utc).AddTicks(5323), "System", "Administration", "MessageLog", null, null },
                    { 3, new DateTime(2025, 1, 30, 8, 18, 27, 146, DateTimeKind.Utc).AddTicks(5325), "System", "Interface", "EmailAccountInterface", null, null }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ExpiresAt", "Password", "RefreshToken", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 30, 9, 18, 27, 146, DateTimeKind.Local).AddTicks(5400), "System", new DateTime(2025, 4, 30, 9, 18, 27, 146, DateTimeKind.Local).AddTicks(5400), "Am1I3JdgO3aS/VUSZ8kfKQ==", "", new DateTime(2025, 1, 30, 9, 18, 27, 146, DateTimeKind.Local).AddTicks(5400), "System" });

            migrationBuilder.InsertData(
                table: "UserAccessRights",
                columns: new[] { "Id", "AccessRightId", "CreatedAt", "CreatedBy", "Deny", "Edit", "UpdatedAt", "UpdatedBy", "UserId", "View" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 30, 8, 18, 27, 148, DateTimeKind.Utc).AddTicks(8510), "System", false, true, null, null, 1, true },
                    { 2, 2, new DateTime(2025, 1, 30, 8, 18, 27, 148, DateTimeKind.Utc).AddTicks(8513), "System", false, true, null, null, 1, true },
                    { 3, 3, new DateTime(2025, 1, 30, 8, 18, 27, 148, DateTimeKind.Utc).AddTicks(8514), "System", false, true, null, null, 1, true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CredentialsId", "Email", "FirstName", "IsActive", "IsNewUserRegistration", "LastName", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, new DateTime(2025, 1, 30, 9, 18, 27, 148, DateTimeKind.Local).AddTicks(8413), "System", 1, "admin.user@gmx.de", "Admin", true, false, "User", 2, new DateTime(2025, 1, 30, 9, 18, 27, 148, DateTimeKind.Local).AddTicks(8413), "System" });

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
