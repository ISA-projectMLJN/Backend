using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Appointment
{
    public partial class appointmen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdministratorsName",
                table: "Appointments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdministratorsSurname",
                table: "Appointments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministratorsName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AdministratorsSurname",
                table: "Appointments");
        }
    }
}
