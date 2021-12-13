using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class SupportInventoryAndDepthMeasurements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventorySpools",
                columns: table => new
                {
                    InventorySpoolId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilamentDefnId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpoolDefnId = table.Column<int>(type: "INTEGER", nullable: false),
                    ColorName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOpened = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsIntrinsic = table.Column<bool>(type: "INTEGER", nullable: false)
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
                        name: "FK_InventorySpools_SpoolDefn_SpoolDefnId",
                        column: x => x.SpoolDefnId,
                        principalTable: "SpoolDefn",
                        principalColumn: "SpoolDefnID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepthMeasurement",
                columns: table => new
                {
                    DepthMeasurementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Depth1 = table.Column<double>(type: "REAL", nullable: false),
                    Depth2 = table.Column<double>(type: "REAL", nullable: false),
                    PercentOffset = table.Column<double>(type: "REAL", nullable: false),
                    AdjustForWind = table.Column<bool>(type: "INTEGER", nullable: false),
                    InventorySpoolId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsIntrinsic = table.Column<bool>(type: "INTEGER", nullable: false)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepthMeasurement");

            migrationBuilder.DropTable(
                name: "InventorySpools");
        }
    }
}
