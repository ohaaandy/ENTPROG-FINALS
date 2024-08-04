﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RunBuddies.DataModel;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240804104729_ADDED_LEADERBOARD_TABLE")]
    partial class ADDED_LEADERBOARD_TABLE
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RunBuddies.DataModel.BuddyInvitation", b =>
                {
                    b.Property<int>("InvitationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvitationID"));

                    b.Property<int>("ReceiverID")
                        .HasColumnType("int");

                    b.Property<int>("SenderID")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("InvitationID");

                    b.ToTable("BuddyInvitations");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyPartner", b =>
                {
                    b.Property<int>("BuddyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BuddyID"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BuddyID");

                    b.ToTable("BuddyPartners");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddySession", b =>
                {
                    b.Property<int>("BuddySessionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BuddySessionID"));

                    b.Property<int>("BuddyID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VerificationID")
                        .HasColumnType("int");

                    b.HasKey("BuddySessionID");

                    b.ToTable("BuddySessions");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Club", b =>
                {
                    b.Property<int>("ClubID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubID"));

                    b.Property<int>("ClubMemberID")
                        .HasColumnType("int");

                    b.Property<int>("ClubModeratorID")
                        .HasColumnType("int");

                    b.Property<string>("ClubName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClubID");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMember", b =>
                {
                    b.Property<int>("ClubMemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubMemberID"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ClubMemberID");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubModerator", b =>
                {
                    b.Property<int>("ClubModeratorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubModeratorID"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ClubModeratorID");

                    b.ToTable("ClubModerators");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Event", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventID"));

                    b.Property<int>("ClubID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LeaderboardID")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("EventID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Leaderboard", b =>
                {
                    b.Property<int>("LeaderboardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaderboardID"));

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<int>("Ranking")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.HasKey("LeaderboardID");

                    b.ToTable("Leaderboards");
                });
#pragma warning restore 612, 618
        }
    }
}
