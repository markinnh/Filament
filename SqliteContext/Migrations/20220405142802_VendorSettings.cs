using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SqliteContext.Migrations
{
    public partial class VendorSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrintSettingDefns",
                columns: table => new
                {
                    PrintSettingDefnId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigItems");

            migrationBuilder.DropTable(
                name: "PrintSettingDefns");

            migrationBuilder.DropTable(
                name: "VendorSettingsConfigs");
        }
    }
}
