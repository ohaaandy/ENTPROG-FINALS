using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    public partial class UpdateEventParticipantsRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Backup existing data
            migrationBuilder.Sql("SELECT * INTO EventParticipants_Backup FROM EventParticipants");

            // Drop existing foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Events_EventID",
                table: "EventParticipants");

            // Drop existing primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants");

            // Drop EventParticipantID column
            migrationBuilder.DropColumn(
                name: "EventParticipantID",
                table: "EventParticipants");

            // Add new primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants",
                columns: new[] { "EventID", "UserID" });

            // Re-add foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Events_EventID",
                table: "EventParticipants",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            // Re-add foreign key for User
            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_AspNetUsers_UserID",
                table: "EventParticipants",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Restore data if needed
            migrationBuilder.Sql("INSERT INTO EventParticipants (EventID, UserID) SELECT EventID, UserID FROM EventParticipants_Backup");

            // Drop backup table
            migrationBuilder.Sql("DROP TABLE EventParticipants_Backup");

            // Remove EventID from AspNetUsers if it was added by mistake
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.Sql("IF COL_LENGTH('AspNetUsers', 'EventID') IS NOT NULL BEGIN ALTER TABLE AspNetUsers DROP COLUMN EventID END");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Backup existing data
            migrationBuilder.Sql("SELECT * INTO EventParticipants_Backup FROM EventParticipants");

            // Drop existing foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Events_EventID",
                table: "EventParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_AspNetUsers_UserID",
                table: "EventParticipants");

            // Drop existing primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants");

            // Add EventParticipantID column back
            migrationBuilder.AddColumn<int>(
                name: "EventParticipantID",
                table: "EventParticipants",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Add primary key on EventParticipantID
            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants",
                column: "EventParticipantID");

            // Re-add index on EventID
            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventID",
                table: "EventParticipants",
                column: "EventID");

            // Re-add foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Events_EventID",
                table: "EventParticipants",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            // Restore data
            migrationBuilder.Sql("INSERT INTO EventParticipants (EventID, UserID) SELECT EventID, UserID FROM EventParticipants_Backup");

            // Drop backup table
            migrationBuilder.Sql("DROP TABLE EventParticipants_Backup");
        }
    }
}