using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    public partial class EditFileMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "CloudFiles");

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "CloudFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "CloudFiles");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CloudFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
