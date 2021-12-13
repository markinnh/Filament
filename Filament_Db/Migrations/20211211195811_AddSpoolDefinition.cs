using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class AddSpoolDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpoolDefn",
                columns: table => new
                {
                    SpoolDefnID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpoolDiameter = table.Column<double>(type: "REAL", nullable: false),
                    DrumDiameter = table.Column<double>(type: "REAL", nullable: false),
                    SpoolWidth = table.Column<double>(type: "REAL", nullable: false),
                    FilamentID = table.Column<int>(type: "INTEGER", nullable: false),
                    StopUsing = table.Column<bool>(type: "INTEGER", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Verified = table.Column<bool>(type: "INTEGER", nullable: false),
                    VendorDefnId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsIntrinsic = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpoolDefn", x => x.SpoolDefnID);
                    table.ForeignKey(
                        name: "FK_SpoolDefn_FilamentDefn_FilamentID",
                        column: x => x.FilamentID,
                        principalTable: "FilamentDefn",
                        principalColumn: "FilamentDefnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpoolDefn_VendorDefns_VendorDefnId",
                        column: x => x.VendorDefnId,
                        principalTable: "VendorDefns",
                        principalColumn: "VendorDefnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpoolDefn_FilamentID",
                table: "SpoolDefn",
                column: "FilamentID");

            migrationBuilder.CreateIndex(
                name: "IX_SpoolDefn_VendorDefnId",
                table: "SpoolDefn",
                column: "VendorDefnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpoolDefn");
        }
    }
}
