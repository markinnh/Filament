using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class UpdateSpoolDefn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpoolDefn_FilamentDefn_FilamentID",
                table: "SpoolDefn");

            migrationBuilder.DropIndex(
                name: "IX_SpoolDefn_FilamentID",
                table: "SpoolDefn");

            migrationBuilder.DropColumn(
                name: "FilamentID",
                table: "SpoolDefn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilamentID",
                table: "SpoolDefn",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpoolDefn_FilamentID",
                table: "SpoolDefn",
                column: "FilamentID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpoolDefn_FilamentDefn_FilamentID",
                table: "SpoolDefn",
                column: "FilamentID",
                principalTable: "FilamentDefn",
                principalColumn: "FilamentDefnId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
