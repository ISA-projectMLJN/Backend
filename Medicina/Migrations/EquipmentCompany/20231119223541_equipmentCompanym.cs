using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.EquipmentCompany
{
    public partial class equipmentCompanym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompanies_Company_CompanyId",
                table: "EquipmentCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompanies_Company_CompanyId1",
                table: "EquipmentCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompanies_Equipment_EquipmentId",
                table: "EquipmentCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCompanies_Equipment_EquipmentId1",
                table: "EquipmentCompanies");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentCompanies",
                table: "EquipmentCompanies");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCompanies_CompanyId",
                table: "EquipmentCompanies");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCompanies_CompanyId1",
                table: "EquipmentCompanies");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCompanies_EquipmentId1",
                table: "EquipmentCompanies");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "EquipmentCompanies");

            migrationBuilder.DropColumn(
                name: "EquipmentId1",
                table: "EquipmentCompanies");

            migrationBuilder.RenameTable(
                name: "EquipmentCompanies",
                newName: "EquipmentCompany");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentCompany",
                table: "EquipmentCompany",
                columns: new[] { "EquipmentId", "CompanyId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentCompany",
                table: "EquipmentCompany");

            migrationBuilder.RenameTable(
                name: "EquipmentCompany",
                newName: "EquipmentCompanies");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "EquipmentCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentId1",
                table: "EquipmentCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentCompanies",
                table: "EquipmentCompanies",
                columns: new[] { "EquipmentId", "CompanyId" });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailablePickupDates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCompanies_CompanyId",
                table: "EquipmentCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCompanies_CompanyId1",
                table: "EquipmentCompanies",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCompanies_EquipmentId1",
                table: "EquipmentCompanies",
                column: "EquipmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_User_CompanyId",
                table: "User",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompanies_Company_CompanyId",
                table: "EquipmentCompanies",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompanies_Company_CompanyId1",
                table: "EquipmentCompanies",
                column: "CompanyId1",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompanies_Equipment_EquipmentId",
                table: "EquipmentCompanies",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCompanies_Equipment_EquipmentId1",
                table: "EquipmentCompanies",
                column: "EquipmentId1",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
