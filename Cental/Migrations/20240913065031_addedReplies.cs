using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cental.Migrations
{
    /// <inheritdoc />
    public partial class addedReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyId",
                table: "Comments",
                column: "ReplyId",
                unique: true,
                filter: "[ReplyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ReplyId",
                table: "Comments",
                column: "ReplyId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ReplyId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReplyId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "OpId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Comments");
        }
    }
}
