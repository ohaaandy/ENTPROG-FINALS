using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClubMemberRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, create the new ClubMemberships table
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubMemberships_Clubs_ClubsClubID",
                        column: x => x.ClubsClubID,
                        principalTable: "Clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Restrict);
                });

            // Populate the new ClubMemberships table with existing relationships
            migrationBuilder.Sql(@"
            INSERT INTO ClubMemberships (ClubsClubID, ClubMembersClubMemberID)
            SELECT ClubID, ClubMemberID 
            FROM Clubs 
            WHERE ClubMemberID IS NOT NULL AND ClubMemberID != 0
        ");

            // Now it's safe to drop the old relationship
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubMemberID",
                table: "Clubs");

            // Update the ClubMembers to AspNetUsers relationship
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberships_ClubsClubID",
                table: "ClubMemberships",
                column: "ClubsClubID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes in reverse order
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.DropTable(
                name: "ClubMemberships");

            migrationBuilder.AddColumn<int>(
                name: "ClubMemberID",
                table: "Clubs",
                type: "int",
                nullable: true);  // Make it nullable to avoid issues with existing data

            // Populate the ClubMemberID column with data from the ClubMemberships table
            migrationBuilder.Sql(@"
            UPDATE c
            SET c.ClubMemberID = cm.ClubMembersClubMemberID
            FROM Clubs c
            JOIN ClubMemberships cm ON c.ClubID = cm.ClubsClubID
        ");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs",
                column: "ClubMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs",
                column: "ClubMemberID",
                principalTable: "ClubMembers",
                principalColumn: "ClubMemberID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}