using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class RefactorTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_MedicId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_PatientId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Treatment",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "MedicId",
                table: "Appointments",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_MedicId",
                table: "Appointments",
                newName: "IX_Appointments_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "MedicId",
                table: "Interventions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Interventions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "Interventions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_MedicId",
                table: "Interventions",
                column: "MedicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Medics_MedicId",
                table: "Interventions",
                column: "MedicId",
                principalTable: "Medics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Medics_MedicId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_MedicId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "MedicId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "Interventions");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Appointments",
                newName: "MedicId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                newName: "IX_Appointments_MedicId");

            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_MedicId",
                table: "Appointments",
                column: "MedicId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
