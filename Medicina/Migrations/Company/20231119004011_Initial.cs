using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Company
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Company");

            migrationBuilder.RenameTable(
                name: "Company",
                newName: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvailablePickupDates",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Companies",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCompany",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCompany_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentCompany_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCompany_CompanyId",
                table: "EquipmentCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCompany_EquipmentId",
                table: "EquipmentCompany",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CompanyId",
                table: "User",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentCompany");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AvailablePickupDates",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Company");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Rate",
                table: "Company",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                table: "Company",
                column: "Id");
        }
    }
}
