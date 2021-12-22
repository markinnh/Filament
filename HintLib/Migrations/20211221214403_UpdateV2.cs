using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HintLib.Migrations
{
    public partial class UpdateV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SavedCrc",
                table: "ProjectFilenames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedCrc",
                table: "ProjectFilenames");
        }
    }
}
