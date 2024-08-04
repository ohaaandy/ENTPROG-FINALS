using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_BUDDYPARTNER_TABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuddyPartners");
        }
    }
}
