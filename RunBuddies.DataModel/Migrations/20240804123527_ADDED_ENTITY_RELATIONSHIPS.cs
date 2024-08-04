using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_ENTITY_RELATIONSHIPS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "ClubModerators",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "ClubMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "BuddyPartners",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "BuddyInvitations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverID",
                table: "BuddyInvitations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboards_EventID",
                table: "Leaderboards",
                column: "EventID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubID",
                table: "Events",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserID",
                table: "Events",
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
                name: "IX_ClubModerators_UserID",
                table: "ClubModerators",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_UserID",
                table: "ClubMembers",
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
                name: "IX_BuddyPartners_UserID",
                table: "BuddyPartners",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BuddyInvitations_ReceiverID",
                table: "BuddyInvitations",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_BuddyInvitations_SenderID",
                table: "BuddyInvitations",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyInvitations_AspNetUsers_ReceiverID",
                table: "BuddyInvitations",
                column: "ReceiverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyInvitations_AspNetUsers_SenderID",
                table: "BuddyInvitations",
                column: "SenderID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserID",
                table: "BuddyPartners",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddySessions_BuddyPartners_BuddyID",
                table: "BuddySessions",
                column: "BuddyID",
                principalTable: "BuddyPartners",
                principalColumn: "BuddyID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddySessions_Verifications_VerificationID",
                table: "BuddySessions",
                column: "VerificationID",
                principalTable: "Verifications",
                principalColumn: "VerificationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubModerators_AspNetUsers_UserID",
                table: "ClubModerators",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs",
                column: "ClubMemberID",
                principalTable: "ClubMembers",
                principalColumn: "ClubMemberID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_ClubModerators_ClubModeratorID",
                table: "Clubs",
                column: "ClubModeratorID",
                principalTable: "ClubModerators",
                principalColumn: "ClubModeratorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_UserID",
                table: "Events",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Clubs_ClubID",
                table: "Events",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ClubID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboards_Events_EventID",
                table: "Leaderboards",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuddyInvitations_AspNetUsers_ReceiverID",
                table: "BuddyInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddyInvitations_AspNetUsers_SenderID",
                table: "BuddyInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserID",
                table: "BuddyPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddySessions_BuddyPartners_BuddyID",
                table: "BuddySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddySessions_Verifications_VerificationID",
                table: "BuddySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubModerators_AspNetUsers_UserID",
                table: "ClubModerators");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubModerators_ClubModeratorID",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_UserID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Clubs_ClubID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboards_Events_EventID",
                table: "Leaderboards");

            migrationBuilder.DropIndex(
                name: "IX_Leaderboards_EventID",
                table: "Leaderboards");

            migrationBuilder.DropIndex(
                name: "IX_Events_ClubID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubModeratorID",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_ClubModerators_UserID",
                table: "ClubModerators");

            migrationBuilder.DropIndex(
                name: "IX_ClubMembers_UserID",
                table: "ClubMembers");

            migrationBuilder.DropIndex(
                name: "IX_BuddySessions_BuddyID",
                table: "BuddySessions");

            migrationBuilder.DropIndex(
                name: "IX_BuddySessions_VerificationID",
                table: "BuddySessions");

            migrationBuilder.DropIndex(
                name: "IX_BuddyPartners_UserID",
                table: "BuddyPartners");

            migrationBuilder.DropIndex(
                name: "IX_BuddyInvitations_ReceiverID",
                table: "BuddyInvitations");

            migrationBuilder.DropIndex(
                name: "IX_BuddyInvitations_SenderID",
                table: "BuddyInvitations");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "ClubModerators",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "ClubMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "BuddyPartners",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "SenderID",
                table: "BuddyInvitations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverID",
                table: "BuddyInvitations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
