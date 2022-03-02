using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SqliteContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilamentDefns",
                columns: table => new
                {
                    FilamentDefnId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(nullable: false),
                    StopUsing = table.Column<bool>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    IsIntrinsic = table.Column<bool>(nullable: false),
                    ShelfLifeInDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentDefns", x => x.FilamentDefnId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    DensityType = table.Column<int>(nullable: false),
                    DefinedDensity = table.Column<double>(nullable: false),
                    FilamentDefnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityAliases", x => x.DensityAliasId);
                    table.ForeignKey(
                        name: "FK_DensityAliases_FilamentDefns_FilamentDefnId",
                        column: x => x.FilamentDefnId,
                        principalTable: "FilamentDefns",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpoolDefns",
                columns: table => new
                {
                    SpoolDefnId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
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
                    table.PrimaryKey("PK_SpoolDefns", x => x.SpoolDefnId);
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    FilamentDefnId = table.Column<int>(nullable: false),
                    SpoolDefnId = table.Column<int>(nullable: false),
                    ColorName = table.Column<string>(nullable: true),
                    DateOpened = table.Column<DateTime>(nullable: false),
                    StopUsing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySpools", x => x.InventorySpoolId);
                    table.ForeignKey(
                        name: "FK_InventorySpools_FilamentDefns_FilamentDefnId",
                        column: x => x.FilamentDefnId,
                        principalTable: "FilamentDefns",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventorySpools_SpoolDefns_SpoolDefnId",
                        column: x => x.SpoolDefnId,
                        principalTable: "SpoolDefns",
                        principalColumn: "SpoolDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepthMeasurements",
                columns: table => new
                {
                    DepthMeasurementId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Depth1 = table.Column<double>(nullable: false),
                    Depth2 = table.Column<double>(nullable: false),
                    MeasureDateTime = table.Column<DateTime>(nullable: false),
                    PercentOffset = table.Column<double>(nullable: false),
                    AdjustForWind = table.Column<bool>(nullable: false),
                    InventorySpoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepthMeasurements", x => x.DepthMeasurementId);
                    table.ForeignKey(
                        name: "FK_DepthMeasurements_InventorySpools_InventorySpoolId",
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
                name: "IX_DepthMeasurements_InventorySpoolId",
                table: "DepthMeasurements",
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
                name: "DepthMeasurements");

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
                name: "FilamentDefns");

            migrationBuilder.DropTable(
                name: "VendorDefns");
        }
    }
}
