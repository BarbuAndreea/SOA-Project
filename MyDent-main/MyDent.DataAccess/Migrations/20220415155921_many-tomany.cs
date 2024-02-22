using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class manytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicPatient_Clinics_ClinicId",
                table: "ClinicPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_ClinicPatient_Patients_PatientId",
                table: "ClinicPatient");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "ClinicPatient",
                newName: "PatientsId");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "ClinicPatient",
                newName: "ClinicsId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicPatient_PatientId",
                table: "ClinicPatient",
                newName: "IX_ClinicPatient_PatientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicPatient_Clinics_ClinicsId",
                table: "ClinicPatient",
                column: "ClinicsId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicPatient_Patients_PatientsId",
                table: "ClinicPatient",
                column: "PatientsId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicPatient_Clinics_ClinicsId",
                table: "ClinicPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_ClinicPatient_Patients_PatientsId",
                table: "ClinicPatient");

            migrationBuilder.RenameColumn(
                name: "PatientsId",
                table: "ClinicPatient",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "ClinicsId",
                table: "ClinicPatient",
                newName: "ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicPatient_PatientsId",
                table: "ClinicPatient",
                newName: "IX_ClinicPatient_PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicPatient_Clinics_ClinicId",
                table: "ClinicPatient",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicPatient_Patients_PatientId",
                table: "ClinicPatient",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
