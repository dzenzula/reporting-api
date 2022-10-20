using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportingApi.Migrations
{
    public partial class Addingatableoffavoritereports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteReports_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteReports_Login_ReportId",
                table: "FavoriteReports",
                columns: new[] { "Login", "ReportId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteReports_ReportId",
                table: "FavoriteReports",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteReports");
        }
    }
}
