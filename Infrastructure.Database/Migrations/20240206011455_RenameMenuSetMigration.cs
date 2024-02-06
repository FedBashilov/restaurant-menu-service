using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    public partial class RenameMenuSetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemCategories_Menu_MenuItemId",
                table: "MenuItemCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menu",
                table: "Menu");

            migrationBuilder.RenameTable(
                name: "Menu",
                newName: "MenuItems");

            migrationBuilder.RenameIndex(
                name: "IX_Menu_ImageId",
                table: "MenuItems",
                newName: "IX_MenuItems_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemCategories_MenuItems_MenuItemId",
                table: "MenuItemCategories",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_CloudFiles_ImageId",
                table: "MenuItems",
                column: "ImageId",
                principalTable: "CloudFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemCategories_MenuItems_MenuItemId",
                table: "MenuItemCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_CloudFiles_ImageId",
                table: "MenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems");

            migrationBuilder.RenameTable(
                name: "MenuItems",
                newName: "Menu");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_ImageId",
                table: "Menu",
                newName: "IX_Menu_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menu",
                table: "Menu",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu",
                column: "ImageId",
                principalTable: "CloudFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemCategories_Menu_MenuItemId",
                table: "MenuItemCategories",
                column: "MenuItemId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
