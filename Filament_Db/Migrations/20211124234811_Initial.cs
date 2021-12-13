using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilamentDefn",
                columns: table => new
                {
                    FilamentDefnId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    StopUsing = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    ShelfLifeInDays = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentDefn", x => x.FilamentDefnId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "DensityAliases",
                columns: table => new
                {
                    DensityAliasId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DensityType = table.Column<int>(type: "INTEGER", nullable: false),
                    DefinedDensity = table.Column<double>(type: "REAL", nullable: false),
                    FilamentDefnId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "MeasuredDensity",
                columns: table => new
                {
                    MeasuredDensityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DensityAliasId = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DensityAliases_FilamentDefnId",
                table: "DensityAliases",
                column: "FilamentDefnId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeasuredDensity_DensityAliasId",
                table: "MeasuredDensity",
                column: "DensityAliasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasuredDensity");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "DensityAliases");

            migrationBuilder.DropTable(
                name: "FilamentDefn");
        }
    }
}
