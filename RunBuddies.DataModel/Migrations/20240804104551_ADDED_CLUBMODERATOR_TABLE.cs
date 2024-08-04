using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_CLUBMODERATOR_TABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubModerators");
        }
    }
}
