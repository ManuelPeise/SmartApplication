using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Databases.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddShareWithAiColumnToEmailMAppingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShareWithAi",
                table: "EmailMappingTable",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareWithAi",
                table: "EmailMappingTable");
        }
    }
}
