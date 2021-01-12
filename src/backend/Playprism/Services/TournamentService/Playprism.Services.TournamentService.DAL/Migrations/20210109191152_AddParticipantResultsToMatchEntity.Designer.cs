﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Playprism.Services.TournamentService.DAL;

namespace Playprism.Services.TournamentService.DAL.Migrations
{
    [DbContext(typeof(TournamentDbContext))]
    [Migration("20210109191152_AddParticipantResultsToMatchEntity")]
    partial class AddParticipantResultsToMatchEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.DisciplineEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("LogoPath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Disciplines");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.GameEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("MatchId")
                        .HasColumnType("integer");

                    b.Property<int>("Score1")
                        .HasColumnType("integer");

                    b.Property<int>("Score2")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.MatchDefinitionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("ConfirmationNeeded")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfGames")
                        .HasColumnType("integer");

                    b.Property<bool>("ScoreBased")
                        .HasColumnType("boolean");

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MatchDefinitions");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.MatchEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("MatchDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Participant1Id")
                        .HasColumnType("integer");

                    b.Property<int?>("Participant1Score")
                        .HasColumnType("integer");

                    b.Property<int?>("Participant2Id")
                        .HasColumnType("integer");

                    b.Property<int?>("Participant2Score")
                        .HasColumnType("integer");

                    b.Property<bool>("Played")
                        .HasColumnType("boolean");

                    b.Property<int?>("PreviousMatch1Id")
                        .HasColumnType("integer");

                    b.Property<int?>("PreviousMatch2Id")
                        .HasColumnType("integer");

                    b.Property<int?>("Result")
                        .HasColumnType("integer");

                    b.Property<int>("RoundId")
                        .HasColumnType("integer");

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.ParticipantEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Approved")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.RoundEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Finished")
                        .HasColumnType("boolean");

                    b.Property<int>("MatchDefinitionId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfCompetitors")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfMatches")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Started")
                        .HasColumnType("boolean");

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MatchDefinitionId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.TournamentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Aborted")
                        .HasColumnType("boolean");

                    b.Property<bool>("AreTeams")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("CheckInDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("CheckInTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("text");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("DisciplineId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Finished")
                        .HasColumnType("boolean");

                    b.Property<int>("Format")
                        .HasColumnType("integer");

                    b.Property<bool>("InviteOnly")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("LogoPath")
                        .HasColumnType("text");

                    b.Property<int>("MaxNumberOfPlayers")
                        .HasColumnType("integer");

                    b.Property<int?>("MaxNumberOfPlayersInTeam")
                        .HasColumnType("integer");

                    b.Property<int?>("MinNumberOfPlayersInTeam")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prizes")
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<bool>("RegistrationApprovalNeeded")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("RegistrationEndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Rules")
                        .HasColumnType("text");

                    b.Property<string>("RulesPath")
                        .HasColumnType("text");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Timezone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Website")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DisciplineId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.GameEntity", b =>
                {
                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.MatchEntity", "Match")
                        .WithMany("Games")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.MatchEntity", b =>
                {
                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.RoundEntity", "Round")
                        .WithMany("Matches")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.TournamentEntity", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Round");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.ParticipantEntity", b =>
                {
                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.TournamentEntity", "Tournament")
                        .WithMany("Participants")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.RoundEntity", b =>
                {
                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.MatchDefinitionEntity", "MatchDefinition")
                        .WithMany()
                        .HasForeignKey("MatchDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MatchDefinition");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.TournamentEntity", b =>
                {
                    b.HasOne("Playprism.Services.TournamentService.DAL.Entities.DisciplineEntity", "Discipline")
                        .WithMany()
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.MatchEntity", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.RoundEntity", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Playprism.Services.TournamentService.DAL.Entities.TournamentEntity", b =>
                {
                    b.Navigation("Participants");
                });
#pragma warning restore 612, 618
        }
    }
}
