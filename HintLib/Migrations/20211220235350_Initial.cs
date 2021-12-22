using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HintLib.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HintsData",
                columns: table => new
                {
                    HintProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SavedCrc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HintsData", x => x.HintProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Hints",
                columns: table => new
                {
                    HintId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HintDataId = table.Column<int>(type: "int", nullable: false),
                    SavedCrc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hints", x => x.HintId);
                    table.ForeignKey(
                        name: "FK_Hints_HintsData_HintDataId",
                        column: x => x.HintDataId,
                        principalTable: "HintsData",
                        principalColumn: "HintProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFilename",
                columns: table => new
                {
                    ProjectFilenameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HintProjectFilename = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HintProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFilename", x => x.ProjectFilenameId);
                    table.ForeignKey(
                        name: "FK_ProjectFilename_HintsData_HintProjectId",
                        column: x => x.HintProjectId,
                        principalTable: "HintsData",
                        principalColumn: "HintProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hints_HintDataId",
                table: "Hints",
                column: "HintDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilename_HintProjectId",
                table: "ProjectFilename",
                column: "HintProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hints");

            migrationBuilder.DropTable(
                name: "ProjectFilename");

            migrationBuilder.DropTable(
                name: "HintsData");
        }
    }
}
