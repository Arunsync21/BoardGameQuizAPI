using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameQuizAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Users",
                newName: "Mobile");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_RoleId",
                table: "UserProgresses",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_RoleId",
                table: "Questions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Roles_RoleId",
                table: "Questions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Roles_RoleId",
                table: "UserProgresses",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Roles_RoleId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Roles_RoleId",
                table: "UserProgresses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_RoleId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Questions_RoleId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Users",
                newName: "MobileNumber");
        }
    }
}
