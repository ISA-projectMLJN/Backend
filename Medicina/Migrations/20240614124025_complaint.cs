using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations
{
    public partial class complaint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    AdministratorId = table.Column<int>(nullable: true),
                    ComplaintText = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    StatusComplaint = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaints");
        }
    }
}
