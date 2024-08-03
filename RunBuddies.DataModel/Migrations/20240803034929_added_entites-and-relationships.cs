using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class added_entitesandrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<int>(type: "int", nullable: false),
                    RunnerLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    VerificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.VerificationID);
                });

            migrationBuilder.CreateTable(
                name: "BuddyPartners",
                columns: table => new
                {
                    BuddyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuddyPartners", x => x.BuddyID);
                    table.ForeignKey(
                        name: "FK_BuddyPartners_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubMembers",
                columns: table => new
                {
                    ClubMemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembers", x => x.ClubMemberID);
                    table.ForeignKey(
                        name: "FK_ClubMembers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubModerators",
                columns: table => new
                {
                    ClubModeratorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubModerators", x => x.ClubModeratorID);
                    table.ForeignKey(
                        name: "FK_ClubModerators_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuddySessions",
                columns: table => new
                {
                    BuddySessionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuddyID = table.Column<int>(type: "int", nullable: false),
                    VerificationID = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuddySessions", x => x.BuddySessionID);
                    table.ForeignKey(
                        name: "FK_BuddySessions_BuddyPartners_BuddyID",
                        column: x => x.BuddyID,
                        principalTable: "BuddyPartners",
                        principalColumn: "BuddyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuddySessions_Verifications_VerificationID",
                        column: x => x.VerificationID,
                        principalTable: "Verifications",
                        principalColumn: "VerificationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubModeratorID = table.Column<int>(type: "int", nullable: false),
                    ClubMemberID = table.Column<int>(type: "int", nullable: false),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubID);
                    table.ForeignKey(
                        name: "FK_Clubs_ClubMembers_ClubMemberID",
                        column: x => x.ClubMemberID,
                        principalTable: "ClubMembers",
                        principalColumn: "ClubMemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clubs_ClubModerators_ClubModeratorID",
                        column: x => x.ClubModeratorID,
                        principalTable: "ClubModerators",
                        principalColumn: "ClubModeratorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaderboardID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboards",
                columns: table => new
                {
                    LeaderboardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Ranking = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboards", x => x.LeaderboardID);
                    table.ForeignKey(
                        name: "FK_Leaderboards_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuddyPartners_UserID",
                table: "BuddyPartners",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BuddySessions_BuddyID",
                table: "BuddySessions",
                column: "BuddyID");

            migrationBuilder.CreateIndex(
                name: "IX_BuddySessions_VerificationID",
                table: "BuddySessions",
                column: "VerificationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_UserID",
                table: "ClubMembers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubModerators_UserID",
                table: "ClubModerators",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs",
                column: "ClubMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubModeratorID",
                table: "Clubs",
                column: "ClubModeratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubID",
                table: "Events",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserID",
                table: "Events",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboards_EventID",
                table: "Leaderboards",
                column: "EventID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuddySessions");

            migrationBuilder.DropTable(
                name: "Leaderboards");

            migrationBuilder.DropTable(
                name: "BuddyPartners");

            migrationBuilder.DropTable(
                name: "Verifications");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "ClubMembers");

            migrationBuilder.DropTable(
                name: "ClubModerators");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
