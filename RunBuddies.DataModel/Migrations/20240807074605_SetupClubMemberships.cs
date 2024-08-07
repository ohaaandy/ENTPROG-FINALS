using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class SetupClubMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create ClubMemberships table for many-to-many relationship
            migrationBuilder.CreateTable(
                name: "ClubMemberships",
                columns: table => new
                {
                    ClubID = table.Column<int>(type: "int", nullable: false),
                    ClubMemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMemberships", x => new { x.ClubID, x.ClubMemberID });
                    table.ForeignKey(
                        name: "FK_ClubMemberships_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubMemberships_ClubMembers_ClubMemberID",
                        column: x => x.ClubMemberID,
                        principalTable: "ClubMembers",
                        principalColumn: "ClubMemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            // Migrate existing relationships to the new table
            migrationBuilder.Sql(@"
                INSERT INTO ClubMemberships (ClubID, ClubMemberID)
                SELECT ClubID, ClubMemberID
                FROM Clubs
                WHERE ClubMemberID IS NOT NULL
            ");

            // Now it's safe to drop the old relationship
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubMembers_ClubMemberID",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubMemberID",
                table: "Clubs");

            // Drop ClubMemberID column from Clubs table
            migrationBuilder.DropColumn(
                name: "ClubMemberID",
                table: "Clubs");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberships_ClubMemberID",
                table: "ClubMemberships",
                column: "ClubMemberID");

            // Ensure ClubMembers has the correct foreign key to AspNetUsers
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubMemberships");

            migrationBuilder.AddColumn<int>(
                name: "ClubMemberID",
                table: "Clubs",
                type: "int",
                nullable: true);

            // Migrate relationships back to the Clubs table
            migrationBuilder.Sql(@"
                UPDATE c
                SET c.ClubMemberID = cm.ClubMemberID
                FROM Clubs c
                JOIN ClubMemberships cm ON c.ClubID = cm.ClubID
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

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserID",
                table: "ClubMembers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
