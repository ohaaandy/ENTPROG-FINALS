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
    [Migration("20240807082500_AddClubMembershipRequestsAndUpdateRelationships")]
    partial class AddClubMembershipRequestsAndUpdateRelationships
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyInvitation", b =>
                {
                    b.Property<int>("InvitationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvitationID"));

                    b.Property<string>("ReceiverID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SenderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("InvitationID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("SenderID");

                    b.ToTable("BuddyInvitations");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyPartner", b =>
                {
                    b.Property<int>("BuddyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BuddyID"));

                    b.Property<string>("User1ID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User2ID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BuddyID");

                    b.HasIndex("User1ID");

                    b.HasIndex("User2ID");

                    b.HasIndex("UserId");

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

                    b.HasIndex("BuddyID");

                    b.HasIndex("VerificationID")
                        .IsUnique();

                    b.ToTable("BuddySessions");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Club", b =>
                {
                    b.Property<int>("ClubID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubID"));

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

                    b.HasIndex("ClubModeratorID");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMember", b =>
                {
                    b.Property<int>("ClubMemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubMemberID"));

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ClubMemberID");

                    b.HasIndex("UserID");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMembership", b =>
                {
                    b.Property<int>("ClubID")
                        .HasColumnType("int");

                    b.Property<int>("ClubMemberID")
                        .HasColumnType("int");

                    b.HasKey("ClubID", "ClubMemberID");

                    b.HasIndex("ClubMemberID");

                    b.ToTable("ClubMemberships", (string)null);
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMembershipRequest", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ClubID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ClubID");

                    b.HasIndex("UserID");

                    b.ToTable("ClubMembershipRequests");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubModerator", b =>
                {
                    b.Property<int>("ClubModeratorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubModeratorID"));

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ClubModeratorID");

                    b.HasIndex("UserID");

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

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EventID");

                    b.HasIndex("ClubID");

                    b.HasIndex("UserID");

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

                    b.HasIndex("EventID")
                        .IsUnique();

                    b.ToTable("Leaderboards");
                });

            modelBuilder.Entity("RunBuddies.DataModel.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Distance")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RunnerLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("Schedule")
                        .HasColumnType("date");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("RunBuddies.DataModel.Verification", b =>
                {
                    b.Property<int>("VerificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VerificationID"));

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.HasKey("VerificationID");

                    b.ToTable("Verifications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyInvitation", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", "Receiver")
                        .WithMany("ReceivedBuddyInvitations")
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", "Sender")
                        .WithMany("SentBuddyInvitations")
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyPartner", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", "User1")
                        .WithMany()
                        .HasForeignKey("User1ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", "User2")
                        .WithMany()
                        .HasForeignKey("User2ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", null)
                        .WithMany("BuddyPartners")
                        .HasForeignKey("UserId");

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddySession", b =>
                {
                    b.HasOne("RunBuddies.DataModel.BuddyPartner", "BuddyPartner")
                        .WithMany("BuddySessions")
                        .HasForeignKey("BuddyID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.Verification", "Verification")
                        .WithOne("BuddySessions")
                        .HasForeignKey("RunBuddies.DataModel.BuddySession", "VerificationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BuddyPartner");

                    b.Navigation("Verification");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Club", b =>
                {
                    b.HasOne("RunBuddies.DataModel.ClubModerator", "ClubModerator")
                        .WithMany("Clubs")
                        .HasForeignKey("ClubModeratorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClubModerator");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMember", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", "User")
                        .WithMany("ClubMembers")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMembership", b =>
                {
                    b.HasOne("RunBuddies.DataModel.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.ClubMember", "ClubMember")
                        .WithMany()
                        .HasForeignKey("ClubMemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("ClubMember");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubMembershipRequest", b =>
                {
                    b.HasOne("RunBuddies.DataModel.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubModerator", b =>
                {
                    b.HasOne("RunBuddies.DataModel.User", "User")
                        .WithMany("ClubModerators")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Event", b =>
                {
                    b.HasOne("RunBuddies.DataModel.Club", "Club")
                        .WithMany("Events")
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RunBuddies.DataModel.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Leaderboard", b =>
                {
                    b.HasOne("RunBuddies.DataModel.Event", "Events")
                        .WithOne("Leaderboards")
                        .HasForeignKey("RunBuddies.DataModel.Leaderboard", "EventID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Events");
                });

            modelBuilder.Entity("RunBuddies.DataModel.BuddyPartner", b =>
                {
                    b.Navigation("BuddySessions");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Club", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("RunBuddies.DataModel.ClubModerator", b =>
                {
                    b.Navigation("Clubs");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Event", b =>
                {
                    b.Navigation("Leaderboards")
                        .IsRequired();
                });

            modelBuilder.Entity("RunBuddies.DataModel.User", b =>
                {
                    b.Navigation("BuddyPartners");

                    b.Navigation("ClubMembers");

                    b.Navigation("ClubModerators");

                    b.Navigation("Events");

                    b.Navigation("ReceivedBuddyInvitations");

                    b.Navigation("SentBuddyInvitations");
                });

            modelBuilder.Entity("RunBuddies.DataModel.Verification", b =>
                {
                    b.Navigation("BuddySessions")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
