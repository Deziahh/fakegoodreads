using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reviewer",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Ratings",
                newName: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Ratings",
                newName: "Score");

            migrationBuilder.AddColumn<string>(
                name: "Reviewer",
                table: "Ratings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
