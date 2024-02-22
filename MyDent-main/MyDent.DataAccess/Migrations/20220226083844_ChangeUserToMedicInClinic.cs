using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class ChangeUserToMedicInClinic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Clinics_ClinicId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClinicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Medics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medics_ClinicId",
                table: "Medics",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics");

            migrationBuilder.DropIndex(
                name: "IX_Medics_ClinicId",
                table: "Medics");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Medics");

            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClinicId",
                table: "Users",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Clinics_ClinicId",
                table: "Users",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }
    }
}
