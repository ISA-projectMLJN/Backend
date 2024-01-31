using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medicina.Migrations.Reservation
{
<<<<<<<< HEAD:Medicina/Migrations/Reservation/20240131201830_reserv.cs
    public partial class reserv : Migration
========
    public partial class res2 : Migration
>>>>>>>> development:Medicina/Migrations/Reservation/20240131131557_res2.cs
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    EquipmentCount = table.Column<int>(nullable: false),
                    IsCollected = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
<<<<<<<< HEAD:Medicina/Migrations/Reservation/20240131201830_reserv.cs
                    CompanyId = table.Column<int>(nullable: false)
========
                    Deadline = table.Column<DateTime>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false)
>>>>>>>> development:Medicina/Migrations/Reservation/20240131131557_res2.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
