using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunicationGroupLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommunicationGroupLink",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationGroupLink",
                table: "Clubs");
        }
    }
}
