using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Appointment
{
    public partial class appointment3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "IsEquipmentTaken",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Appointments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEquipmentTaken",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Appointments",
                type: "int",
                nullable: true);
        }
    }
}
