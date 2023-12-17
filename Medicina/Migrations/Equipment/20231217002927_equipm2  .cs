using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Equipment
{
    public partial class equipm2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Equipment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Equipment");
        }
    }
}
