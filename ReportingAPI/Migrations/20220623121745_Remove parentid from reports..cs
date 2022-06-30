using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class Removeparentidfromreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Reports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Reports",
                type: "int",
                nullable: true);
        }
    }
}
