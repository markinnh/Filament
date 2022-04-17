using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class VendorSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DensityAliases_FilamentDefn_FilamentDefnId",
                table: "DensityAliases");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_DepthMeasurement_InventorySpools_InventorySpoolId",
            //    table: "DepthMeasurement");

            migrationBuilder.DropForeignKey(
                name: "FK_InventorySpools_FilamentDefn_FilamentDefnId",
                table: "InventorySpools");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FilamentDefn",
            //    table: "FilamentDefn");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_DepthMeasurement",
            //    table: "DepthMeasurement");

            //migrationBuilder.RenameTable(
            //    name: "FilamentDefn",
            //    newName: "FilamentDefns");

            //migrationBuilder.RenameTable(
            //    name: "DepthMeasurement",
            //    newName: "DepthMeasurements");

            migrationBuilder.RenameColumn(
                name: "SpoolDefnID",
                table: "SpoolDefns",
                newName: "SpoolDefnId");

            migrationBuilder.RenameIndex(
                name: "IX_DepthMeasurement_InventorySpoolId",
                table: "DepthMeasurements",
                newName: "IX_DepthMeasurements_InventorySpoolId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Settings",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "InventorySpools",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_FilamentDefns",
            //    table: "FilamentDefns",
            //    column: "FilamentDefnId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_DepthMeasurements",
            //    table: "DepthMeasurements",
            //    column: "DepthMeasurementId");

            migrationBuilder.CreateTable(
                name: "PrintSettingDefns",
                columns: table => new
                {
                    PrintSettingDefnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(maxLength: 128, nullable: true),
                    SettingValueType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintSettingDefns", x => x.PrintSettingDefnId);
                });

            migrationBuilder.CreateTable(
                name: "VendorSettingsConfigs",
                columns: table => new
                {
                    VendorSettingsConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorDefnId = table.Column<int>(nullable: false),
                    FilamentDefnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorSettingsConfigs", x => x.VendorSettingsConfigId);
                    table.ForeignKey(
                        name: "FK_VendorSettingsConfigs_FilamentDefns_FilamentDefnId",
                        column: x => x.FilamentDefnId,
                        principalTable: "FilamentDefns",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorSettingsConfigs_VendorDefns_VendorDefnId",
                        column: x => x.VendorDefnId,
                        principalTable: "VendorDefns",
                        principalColumn: "VendorDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigItems",
                columns: table => new
                {
                    ConfigItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorSettingsConfigId = table.Column<int>(nullable: false),
                    PrintSettingDefnId = table.Column<int>(nullable: false),
                    DateEntered = table.Column<DateTime>(nullable: false),
                    TextValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItems", x => x.ConfigItemId);
                    table.ForeignKey(
                        name: "FK_ConfigItems_PrintSettingDefns_PrintSettingDefnId",
                        column: x => x.PrintSettingDefnId,
                        principalTable: "PrintSettingDefns",
                        principalColumn: "PrintSettingDefnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigItems_VendorSettingsConfigs_VendorSettingsConfigId",
                        column: x => x.VendorSettingsConfigId,
                        principalTable: "VendorSettingsConfigs",
                        principalColumn: "VendorSettingsConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_PrintSettingDefnId",
                table: "ConfigItems",
                column: "PrintSettingDefnId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_VendorSettingsConfigId",
                table: "ConfigItems",
                column: "VendorSettingsConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorSettingsConfigs_FilamentDefnId",
                table: "VendorSettingsConfigs",
                column: "FilamentDefnId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorSettingsConfigs_VendorDefnId",
                table: "VendorSettingsConfigs",
                column: "VendorDefnId");

            migrationBuilder.AddForeignKey(
                name: "FK_DensityAliases_FilamentDefns_FilamentDefnId",
                table: "DensityAliases",
                column: "FilamentDefnId",
                principalTable: "FilamentDefns",
                principalColumn: "FilamentDefnId",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DepthMeasurements_InventorySpools_InventorySpoolId",
            //    table: "DepthMeasurements",
            //    column: "InventorySpoolId",
            //    principalTable: "InventorySpools",
            //    principalColumn: "InventorySpoolId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_InventorySpools_FilamentDefns_FilamentDefnId",
            //    table: "InventorySpools",
            //    column: "FilamentDefnId",
            //    principalTable: "FilamentDefns",
            //    principalColumn: "FilamentDefnId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DensityAliases_FilamentDefns_FilamentDefnId",
                table: "DensityAliases");

            migrationBuilder.DropForeignKey(
                name: "FK_DepthMeasurements_InventorySpools_InventorySpoolId",
                table: "DepthMeasurements");

            migrationBuilder.DropForeignKey(
                name: "FK_InventorySpools_FilamentDefns_FilamentDefnId",
                table: "InventorySpools");

            migrationBuilder.DropTable(
                name: "ConfigItems");

            migrationBuilder.DropTable(
                name: "PrintSettingDefns");

            migrationBuilder.DropTable(
                name: "VendorSettingsConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilamentDefns",
                table: "FilamentDefns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepthMeasurements",
                table: "DepthMeasurements");

            migrationBuilder.RenameTable(
                name: "FilamentDefns",
                newName: "FilamentDefn");

            migrationBuilder.RenameTable(
                name: "DepthMeasurements",
                newName: "DepthMeasurement");

            migrationBuilder.RenameColumn(
                name: "SpoolDefnId",
                table: "SpoolDefns",
                newName: "SpoolDefnID");

            migrationBuilder.RenameIndex(
                name: "IX_DepthMeasurements_InventorySpoolId",
                table: "DepthMeasurement",
                newName: "IX_DepthMeasurement_InventorySpoolId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "InventorySpools",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilamentDefn",
                table: "FilamentDefn",
                column: "FilamentDefnId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepthMeasurement",
                table: "DepthMeasurement",
                column: "DepthMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_DensityAliases_FilamentDefn_FilamentDefnId",
                table: "DensityAliases",
                column: "FilamentDefnId",
                principalTable: "FilamentDefn",
                principalColumn: "FilamentDefnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepthMeasurement_InventorySpools_InventorySpoolId",
                table: "DepthMeasurement",
                column: "InventorySpoolId",
                principalTable: "InventorySpools",
                principalColumn: "InventorySpoolId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventorySpools_FilamentDefn_FilamentDefnId",
                table: "InventorySpools",
                column: "FilamentDefnId",
                principalTable: "FilamentDefn",
                principalColumn: "FilamentDefnId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
