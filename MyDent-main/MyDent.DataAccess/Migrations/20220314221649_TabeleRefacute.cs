using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class TabeleRefacute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Medics_MedicId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Appointments_AppointmentId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Medics_MedicId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_AppointmentId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_MedicId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Interventions");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Medics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MedicId",
                table: "Interventions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MedicId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Medics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MedicId",
                table: "Interventions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Interventions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MedicId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_AppointmentId",
                table: "Interventions",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_MedicId",
                table: "Interventions",
                column: "MedicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicId",
                table: "Appointments",
                column: "MedicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Medics_MedicId",
                table: "Appointments",
                column: "MedicId",
                principalTable: "Medics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Appointments_AppointmentId",
                table: "Interventions",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Medics_MedicId",
                table: "Interventions",
                column: "MedicId",
                principalTable: "Medics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_Clinics_ClinicId",
                table: "Medics",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }
    }
}
