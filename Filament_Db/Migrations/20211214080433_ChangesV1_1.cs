using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class ChangesV1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIntrinsic",
                table: "VendorDefns");

            migrationBuilder.DropColumn(
                name: "IsIntrinsic",
                table: "SpoolDefn");

            migrationBuilder.DropColumn(
                name: "IsIntrinsic",
                table: "DepthMeasurement");

            migrationBuilder.RenameColumn(
                name: "IsIntrinsic",
                table: "InventorySpools",
                newName: "StopUsing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StopUsing",
                table: "InventorySpools",
                newName: "IsIntrinsic");

            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsic",
                table: "VendorDefns",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsic",
                table: "SpoolDefn",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsic",
                table: "DepthMeasurement",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
