using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilamentDefn",
                columns: table => new
                {
                    FilamentDefnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diameter = table.Column<double>(nullable: false),
                    StopUsing = table.Column<bool>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    IsIntrinsic = table.Column<bool>(nullable: false),
                    ShelfLifeInDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentDefn", x => x.FilamentDefnId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "VendorDefns",
                columns: table => new
                {
                    VendorDefnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    FoundOnAmazon = table.Column<bool>(nullable: false),
                    WebUrl = table.Column<string>(nullable: true),
                    StopUsing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorDefns", x => x.VendorDefnId);
                });

            migrationBuilder.CreateTable(
                name: "DensityAliases",
                columns: table => new
                {
                    DensityAliasId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DensityType = table.Column<int>(nullable: false),
                    DefinedDensity = table.Column<double>(nullable: false),
                    FilamentDefnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityAliases", x => x.DensityAliasId);
                    table.ForeignKey(
                        name: "FK_DensityAliases_FilamentDefn_FilamentDefnId",
                        column: x => x.FilamentDefnId,
                        principalTable: "FilamentDefn",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpoolDefns",
                columns: table => new
                {
                    SpoolDefnID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpoolDiameter = table.Column<double>(nullable: false),
                    DrumDiameter = table.Column<double>(nullable: false),
                    SpoolWidth = table.Column<double>(nullable: false),
                    StopUsing = table.Column<bool>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Verified = table.Column<bool>(nullable: false),
                    VendorDefnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpoolDefns", x => x.SpoolDefnID);
                    table.ForeignKey(
                        name: "FK_SpoolDefns_VendorDefns_VendorDefnId",
                        column: x => x.VendorDefnId,
                        principalTable: "VendorDefns",
                        principalColumn: "VendorDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasuredDensity",
                columns: table => new
                {
                    MeasuredDensityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DensityAliasId = table.Column<int>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    Diameter = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasuredDensity", x => x.MeasuredDensityId);
                    table.ForeignKey(
                        name: "FK_MeasuredDensity_DensityAliases_DensityAliasId",
                        column: x => x.DensityAliasId,
                        principalTable: "DensityAliases",
                        principalColumn: "DensityAliasId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventorySpools",
                columns: table => new
                {
                    InventorySpoolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilamentDefnId = table.Column<int>(nullable: false),
                    SpoolDefnId = table.Column<int>(nullable: false),
                    ColorName = table.Column<string>(nullable: false),
                    DateOpened = table.Column<DateTime>(nullable: false),
                    StopUsing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySpools", x => x.InventorySpoolId);
                    table.ForeignKey(
                        name: "FK_InventorySpools_FilamentDefn_FilamentDefnId",
                        column: x => x.FilamentDefnId,
                        principalTable: "FilamentDefn",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventorySpools_SpoolDefns_SpoolDefnId",
                        column: x => x.SpoolDefnId,
                        principalTable: "SpoolDefns",
                        principalColumn: "SpoolDefnID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepthMeasurement",
                columns: table => new
                {
                    DepthMeasurementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depth1 = table.Column<double>(nullable: false),
                    Depth2 = table.Column<double>(nullable: false),
                    MeasureDateTime = table.Column<DateTime>(nullable: false),
                    PercentOffset = table.Column<double>(nullable: false),
                    AdjustForWind = table.Column<bool>(nullable: false),
                    InventorySpoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepthMeasurement", x => x.DepthMeasurementId);
                    table.ForeignKey(
                        name: "FK_DepthMeasurement_InventorySpools_InventorySpoolId",
                        column: x => x.InventorySpoolId,
                        principalTable: "InventorySpools",
                        principalColumn: "InventorySpoolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DensityAliases_FilamentDefnId",
                table: "DensityAliases",
                column: "FilamentDefnId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepthMeasurement_InventorySpoolId",
                table: "DepthMeasurement",
                column: "InventorySpoolId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySpools_FilamentDefnId",
                table: "InventorySpools",
                column: "FilamentDefnId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySpools_SpoolDefnId",
                table: "InventorySpools",
                column: "SpoolDefnId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasuredDensity_DensityAliasId",
                table: "MeasuredDensity",
                column: "DensityAliasId");

            migrationBuilder.CreateIndex(
                name: "IX_SpoolDefns_VendorDefnId",
                table: "SpoolDefns",
                column: "VendorDefnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepthMeasurement");

            migrationBuilder.DropTable(
                name: "MeasuredDensity");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "InventorySpools");

            migrationBuilder.DropTable(
                name: "DensityAliases");

            migrationBuilder.DropTable(
                name: "SpoolDefns");

            migrationBuilder.DropTable(
                name: "FilamentDefn");

            migrationBuilder.DropTable(
                name: "VendorDefns");
        }
    }
}
