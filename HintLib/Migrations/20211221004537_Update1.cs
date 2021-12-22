using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HintLib.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilename_HintsData_HintProjectId",
                table: "ProjectFilename");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectFilename",
                table: "ProjectFilename");

            migrationBuilder.RenameTable(
                name: "ProjectFilename",
                newName: "ProjectFilenames");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectFilename_HintProjectId",
                table: "ProjectFilenames",
                newName: "IX_ProjectFilenames_HintProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectFilenames",
                table: "ProjectFilenames",
                column: "ProjectFilenameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilenames_HintsData_HintProjectId",
                table: "ProjectFilenames",
                column: "HintProjectId",
                principalTable: "HintsData",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilenames_HintsData_HintProjectId",
                table: "ProjectFilenames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectFilenames",
                table: "ProjectFilenames");

            migrationBuilder.RenameTable(
                name: "ProjectFilenames",
                newName: "ProjectFilename");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectFilenames_HintProjectId",
                table: "ProjectFilename",
                newName: "IX_ProjectFilename_HintProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectFilename",
                table: "ProjectFilename",
                column: "ProjectFilenameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilename_HintsData_HintProjectId",
                table: "ProjectFilename",
                column: "HintProjectId",
                principalTable: "HintsData",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
