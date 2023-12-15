using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Equipment
{
    public partial class equipm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Equipment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Equipment");
        }
    }
}
