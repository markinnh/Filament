using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class SupportIntrinsic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsic",
                table: "VendorDefns",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsic",
                table: "FilamentDefn",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIntrinsic",
                table: "VendorDefns");

            migrationBuilder.DropColumn(
                name: "IsIntrinsic",
                table: "FilamentDefn");
        }
    }
}
