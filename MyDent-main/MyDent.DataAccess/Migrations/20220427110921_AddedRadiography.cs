using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class AddedRadiography : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Radiography_Patients_PatientId",
                table: "Radiography");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Radiography",
                table: "Radiography");

            migrationBuilder.RenameTable(
                name: "Radiography",
                newName: "Radiographies");

            migrationBuilder.RenameIndex(
                name: "IX_Radiography_PatientId",
                table: "Radiographies",
                newName: "IX_Radiographies_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Radiographies",
                table: "Radiographies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Radiographies_Patients_PatientId",
                table: "Radiographies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Radiographies_Patients_PatientId",
                table: "Radiographies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Radiographies",
                table: "Radiographies");

            migrationBuilder.RenameTable(
                name: "Radiographies",
                newName: "Radiography");

            migrationBuilder.RenameIndex(
                name: "IX_Radiographies_PatientId",
                table: "Radiography",
                newName: "IX_Radiography_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Radiography",
                table: "Radiography",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Radiography_Patients_PatientId",
                table: "Radiography",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
