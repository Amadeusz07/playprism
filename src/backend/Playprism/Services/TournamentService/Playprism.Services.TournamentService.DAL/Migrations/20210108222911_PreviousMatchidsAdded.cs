using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    public partial class PreviousMatchidsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousMatch1Id",
                table: "Matches",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousMatch2Id",
                table: "Matches",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousMatch1Id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "PreviousMatch2Id",
                table: "Matches");
        }
    }
}
