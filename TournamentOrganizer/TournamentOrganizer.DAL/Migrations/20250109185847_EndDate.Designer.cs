﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TournamentOrganizer.DAL;

#nullable disable

namespace TournamentOrganizer.DAL.Migrations
{
    [DbContext(typeof(TournamentContext))]
    [Migration("20250109185847_EndDate")]
    partial class EndDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MatchNumber")
                        .HasColumnType("int");

                    b.Property<Guid?>("Participant1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Participant2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoundId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Participant1Id");

                    b.HasIndex("Participant2Id");

                    b.HasIndex("RoundId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Participant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Round", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoundNumber")
                        .HasColumnType("int");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Match", b =>
                {
                    b.HasOne("TournamentOrganizer.DAL.Entities.Participant", "Participant1")
                        .WithMany("MatchesAsParticipant1")
                        .HasForeignKey("Participant1Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TournamentOrganizer.DAL.Entities.Participant", "Participant2")
                        .WithMany("MatchesAsParticipant2")
                        .HasForeignKey("Participant2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TournamentOrganizer.DAL.Entities.Round", "Round")
                        .WithMany("Matches")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentOrganizer.DAL.Entities.Participant", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Participant1");

                    b.Navigation("Participant2");

                    b.Navigation("Round");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Participant", b =>
                {
                    b.HasOne("TournamentOrganizer.DAL.Entities.Tournament", "Tournament")
                        .WithMany("Participants")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Round", b =>
                {
                    b.HasOne("TournamentOrganizer.DAL.Entities.Tournament", "Tournament")
                        .WithMany("Rounds")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Participant", b =>
                {
                    b.Navigation("MatchesAsParticipant1");

                    b.Navigation("MatchesAsParticipant2");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Round", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("TournamentOrganizer.DAL.Entities.Tournament", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("Rounds");
                });
#pragma warning restore 612, 618
        }
    }
}
