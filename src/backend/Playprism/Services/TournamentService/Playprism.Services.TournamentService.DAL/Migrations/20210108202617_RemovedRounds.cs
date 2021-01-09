using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    public partial class RemovedRounds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextRoundId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "PreviousRoundId",
                table: "Rounds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextRoundId",
                table: "Rounds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousRoundId",
                table: "Rounds",
                type: "integer",
                nullable: true);
        }
    }
}
