using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    public partial class Tournaments_Ongoing_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ongoing",
                table: "Tournaments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ongoing",
                table: "Tournaments");
        }
    }
}
