using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class updatingschema5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Reports",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Categories",
                newName: "UniqueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "Reports",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "Categories",
                newName: "OrderId");
        }
    }
}
