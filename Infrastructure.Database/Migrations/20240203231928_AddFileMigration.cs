using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    public partial class AddFileMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Menu");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CloudFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ImageId",
                table: "Menu",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu",
                column: "ImageId",
                principalTable: "CloudFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_CloudFiles_ImageId",
                table: "Menu");

            migrationBuilder.DropTable(
                name: "CloudFiles");

            migrationBuilder.DropIndex(
                name: "IX_Menu_ImageId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Menu");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
