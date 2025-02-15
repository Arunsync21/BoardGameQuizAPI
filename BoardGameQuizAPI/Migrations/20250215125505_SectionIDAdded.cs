using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameQuizAPI.Migrations
{
    /// <inheritdoc />
    public partial class SectionIDAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SetId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SetId",
                table: "Questions",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Sets_SetId",
                table: "Questions",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "SetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Sets_SetId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SetId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Questions");
        }
    }
}
