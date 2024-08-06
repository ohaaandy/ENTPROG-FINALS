using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClubMemberRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubMemberID",
                table: "Clubs");

            migrationBuilder.CreateTable(
                name: "ClubMemberships",
                columns: table => new
                {
                    ClubMembersClubMemberID = table.Column<int>(type: "int", nullable: false),
                    ClubsClubID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMemberships", x => new { x.ClubMembersClubMemberID, x.ClubsClubID });
                    table.ForeignKey(
                        name: "FK_ClubMemberships_ClubMembers_ClubMembersClubMemberID",
                        column: x => x.ClubMembersClubMemberID,
                        principalTable: "ClubMembers",
                        principalColumn: "ClubMemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubMemberships_Clubs_ClubsClubID",
                        column: x => x.ClubsClubID,
                        principalTable: "Clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberships_ClubsClubID",
                table: "ClubMemberships",
                column: "ClubsClubID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.DropTable(
                name: "ClubMemberships");

            migrationBuilder.AddColumn<int>(
                name: "ClubMemberID",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs",
                column: "ClubMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
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
        }
    }
}
