using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TeamService.API.Migrations
{
    public partial class ChangedTeamAssignementPropertyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayerAssignmentEntity_Players_PlayerId",
                table: "TeamPlayerAssignmentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayerAssignmentEntity_Teams_TeamId",
                table: "TeamPlayerAssignmentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayerAssignmentEntity",
                table: "TeamPlayerAssignmentEntity");

            migrationBuilder.RenameTable(
                name: "TeamPlayerAssignmentEntity",
                newName: "TeamPlayerAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayerAssignmentEntity_PlayerId",
                table: "TeamPlayerAssignments",
                newName: "IX_TeamPlayerAssignments_PlayerId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayerAssignments",
                table: "TeamPlayerAssignments",
                columns: new[] { "TeamId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayerAssignments_Players_PlayerId",
                table: "TeamPlayerAssignments",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayerAssignments_Teams_TeamId",
                table: "TeamPlayerAssignments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayerAssignments_Players_PlayerId",
                table: "TeamPlayerAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayerAssignments_Teams_TeamId",
                table: "TeamPlayerAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayerAssignments",
                table: "TeamPlayerAssignments");

            migrationBuilder.RenameTable(
                name: "TeamPlayerAssignments",
                newName: "TeamPlayerAssignmentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayerAssignments_PlayerId",
                table: "TeamPlayerAssignmentEntity",
                newName: "IX_TeamPlayerAssignmentEntity_PlayerId");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayerAssignmentEntity",
                table: "TeamPlayerAssignmentEntity",
                columns: new[] { "TeamId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayerAssignmentEntity_Players_PlayerId",
                table: "TeamPlayerAssignmentEntity",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayerAssignmentEntity_Teams_TeamId",
                table: "TeamPlayerAssignmentEntity",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
