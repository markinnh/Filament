using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filament_Db.Migrations
{
    public partial class AddVendorDefn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorDefns",
                columns: table => new
                {
                    VendorDefnId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FoundOnAmazon = table.Column<bool>(type: "INTEGER", nullable: false),
                    WebUrl = table.Column<string>(type: "TEXT", nullable: false),
                    StopUsing = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorDefns", x => x.VendorDefnId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorDefns");
        }
    }
}
