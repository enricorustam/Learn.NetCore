using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updateEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_M_Employee",
                columns: table => new
                {
                    EmpId = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CreatedData = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedData = table.Column<DateTimeOffset>(nullable: false),
                    DeletedData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Employee", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_TB_M_Employee_TB_M_User_EmpId",
                        column: x => x.EmpId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_M_Employee");
        }
    }
}
