using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class RenamingCreatedOnCreatedOnfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Reports",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Reports",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Categories",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Reports",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reports",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Categories",
                newName: "CreatedOn");
        }
    }
}
