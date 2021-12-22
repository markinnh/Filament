using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HintLib.Migrations
{
    public partial class UpdateV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hints_HintsData_HintDataId",
                table: "Hints");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilenames_HintsData_HintProjectId",
                table: "ProjectFilenames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HintsData",
                table: "HintsData");

            migrationBuilder.RenameTable(
                name: "HintsData",
                newName: "HintProjects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HintProjects",
                table: "HintProjects",
                column: "HintProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hints_HintProjects_HintDataId",
                table: "Hints",
                column: "HintDataId",
                principalTable: "HintProjects",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilenames_HintProjects_HintProjectId",
                table: "ProjectFilenames",
                column: "HintProjectId",
                principalTable: "HintProjects",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hints_HintProjects_HintDataId",
                table: "Hints");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilenames_HintProjects_HintProjectId",
                table: "ProjectFilenames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HintProjects",
                table: "HintProjects");

            migrationBuilder.RenameTable(
                name: "HintProjects",
                newName: "HintsData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HintsData",
                table: "HintsData",
                column: "HintProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hints_HintsData_HintDataId",
                table: "Hints",
                column: "HintDataId",
                principalTable: "HintsData",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilenames_HintsData_HintProjectId",
                table: "ProjectFilenames",
                column: "HintProjectId",
                principalTable: "HintsData",
                principalColumn: "HintProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
