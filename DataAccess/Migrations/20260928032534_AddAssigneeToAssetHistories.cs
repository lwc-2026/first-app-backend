using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAssigneeToAssetHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "AssetHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistories_AssignedUserId",
                table: "AssetHistories",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistories_Users_AssignedUserId",
                table: "AssetHistories",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistories_Users_AssignedUserId",
                table: "AssetHistories");

            migrationBuilder.DropIndex(
                name: "IX_AssetHistories_AssignedUserId",
                table: "AssetHistories");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "AssetHistories");
        }
    }
}
