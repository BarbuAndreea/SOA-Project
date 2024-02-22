using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class Unique_Personal_Code_For_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalCode",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonalCode",
                table: "Users",
                column: "PersonalCode",
                unique: true,
                filter: "[PersonalCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PersonalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonalCode",
                table: "Users");
        }
    }
}
