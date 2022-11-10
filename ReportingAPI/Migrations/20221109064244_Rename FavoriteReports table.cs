using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportingApi.Migrations
{
    public partial class RenameFavoriteReportstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "FavoriteReports",
                newName: "FavoriteReports",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FavoriteReports",
                schema: "dbo",
                newName: "FavoriteReports");
        }
    }
}
