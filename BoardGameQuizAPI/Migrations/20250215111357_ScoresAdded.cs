using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameQuizAPI.Migrations
{
    /// <inheritdoc />
    public partial class ScoresAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScoreObtained",
                table: "UserProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreObtained",
                table: "UserProgresses");
        }
    }
}
