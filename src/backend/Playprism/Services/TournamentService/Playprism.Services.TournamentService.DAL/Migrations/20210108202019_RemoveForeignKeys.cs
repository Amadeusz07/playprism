using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    public partial class RemoveForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Rounds_NextRoundId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Rounds_PreviousRoundId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_NextRoundId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_PreviousRoundId",
                table: "Rounds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rounds_NextRoundId",
                table: "Rounds",
                column: "NextRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_PreviousRoundId",
                table: "Rounds",
                column: "PreviousRoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Rounds_NextRoundId",
                table: "Rounds",
                column: "NextRoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Rounds_PreviousRoundId",
                table: "Rounds",
                column: "PreviousRoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
