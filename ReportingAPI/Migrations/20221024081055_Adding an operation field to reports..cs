using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportingApi.Migrations
{
    public partial class Addinganoperationfieldtoreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Operation_name",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operation_name",
                table: "Reports");
        }
    }
}
