using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDent.DataAccess.Migrations
{
    public partial class PathToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Radiographies",
                newName: "Image64");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Radiographies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Radiographies");

            migrationBuilder.RenameColumn(
                name: "Image64",
                table: "Radiographies",
                newName: "Path");
        }
    }
}
