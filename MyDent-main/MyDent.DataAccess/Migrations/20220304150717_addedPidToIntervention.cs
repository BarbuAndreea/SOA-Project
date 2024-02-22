using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class addedPidToIntervention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Patients_PatientId",
                table: "Interventions");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Interventions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Patients_PatientId",
                table: "Interventions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Patients_PatientId",
                table: "Interventions");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Interventions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Patients_PatientId",
                table: "Interventions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
