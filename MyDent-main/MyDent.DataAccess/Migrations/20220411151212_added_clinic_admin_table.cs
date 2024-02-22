using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class added_clinic_admin_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClinicAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCId = table.Column<int>(type: "int", nullable: true),
                    ClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicAdmins_Users_UserCId",
                        column: x => x.UserCId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAdmins_UserCId",
                table: "ClinicAdmins",
                column: "UserCId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicAdmins");
        }
    }
}
