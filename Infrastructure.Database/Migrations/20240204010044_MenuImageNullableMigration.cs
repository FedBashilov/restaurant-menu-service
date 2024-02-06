using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    public partial class MenuImageNullableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Menu",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu",
                column: "ImageId",
                principalTable: "CloudFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu",
                column: "ImageId",
                principalTable: "CloudFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
