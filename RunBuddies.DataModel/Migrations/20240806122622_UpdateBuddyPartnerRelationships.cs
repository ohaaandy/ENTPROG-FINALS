using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunBuddies.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBuddyPartnerRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserID",
                table: "BuddyPartners");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "BuddyPartners",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BuddyPartners_UserID",
                table: "BuddyPartners",
                newName: "IX_BuddyPartners_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BuddyPartners",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "User1ID",
                table: "BuddyPartners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User2ID",
                table: "BuddyPartners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BuddyPartners_User1ID",
                table: "BuddyPartners",
                column: "User1ID");

            migrationBuilder.CreateIndex(
                name: "IX_BuddyPartners_User2ID",
                table: "BuddyPartners",
                column: "User2ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_User1ID",
                table: "BuddyPartners",
                column: "User1ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_User2ID",
                table: "BuddyPartners",
                column: "User2ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserId",
                table: "BuddyPartners",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_User1ID",
                table: "BuddyPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_User2ID",
                table: "BuddyPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserId",
                table: "BuddyPartners");

            migrationBuilder.DropIndex(
                name: "IX_BuddyPartners_User1ID",
                table: "BuddyPartners");

            migrationBuilder.DropIndex(
                name: "IX_BuddyPartners_User2ID",
                table: "BuddyPartners");

            migrationBuilder.DropColumn(
                name: "User1ID",
                table: "BuddyPartners");

            migrationBuilder.DropColumn(
                name: "User2ID",
                table: "BuddyPartners");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BuddyPartners",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_BuddyPartners_UserId",
                table: "BuddyPartners",
                newName: "IX_BuddyPartners_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "BuddyPartners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BuddyPartners_AspNetUsers_UserID",
                table: "BuddyPartners",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
