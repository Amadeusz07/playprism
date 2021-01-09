using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    public partial class MinorAdjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfPlayers",
                table: "Rounds",
                newName: "NumberOfCompetitors");

            migrationBuilder.AlterColumn<string>(
                name: "Timezone",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Started",
                table: "Rounds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Participants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Started",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "NumberOfCompetitors",
                table: "Rounds",
                newName: "NumberOfPlayers");

            migrationBuilder.AlterColumn<string>(
                name: "Timezone",
                table: "Tournaments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Tournaments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tournaments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
