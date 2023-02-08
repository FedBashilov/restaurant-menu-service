using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Facade.Migrations
{
    public partial class AddVisibleFlagMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Menu",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Menu");
        }
    }
}
