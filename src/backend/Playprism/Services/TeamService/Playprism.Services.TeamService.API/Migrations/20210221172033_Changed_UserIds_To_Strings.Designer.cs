﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Playprism.Services.TeamService.API.Repositories;

namespace Playprism.Services.TeamService.API.Migrations
{
    [DbContext(typeof(TeamDbContext))]
    [Migration("20210221172033_Changed_UserIds_To_Strings")]
    partial class Changed_UserIds_To_Strings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.PlayerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("Name")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.TeamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Contact")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("LogoPath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.TeamPlayerAssignmentEntity", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<bool>("Accepted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("InviteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("LeaveDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("ResponseDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("TeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("TeamPlayerAssignmentEntity");
                });

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.TeamPlayerAssignmentEntity", b =>
                {
                    b.HasOne("Playprism.Services.TeamService.API.Entities.PlayerEntity", "Player")
                        .WithMany("Assignments")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playprism.Services.TeamService.API.Entities.TeamEntity", "Team")
                        .WithMany("Assignments")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.PlayerEntity", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Playprism.Services.TeamService.API.Entities.TeamEntity", b =>
                {
                    b.Navigation("Assignments");
                });
#pragma warning restore 612, 618
        }
    }
}