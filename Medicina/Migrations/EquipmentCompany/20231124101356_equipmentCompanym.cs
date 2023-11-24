using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.EquipmentCompany
{
    public partial class equipmentCompanym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentCompany",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCompany", x => new { x.EquipmentId, x.CompanyId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentCompany");
        }
    }
}
