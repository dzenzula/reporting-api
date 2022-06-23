using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class Addingamanytomanyrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Categories_ParentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ParentId",
                table: "Reports");

            migrationBuilder.CreateTable(
                name: "CategoryReports",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ReportsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryReports", x => new { x.CategoriesId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_CategoryReports_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryReports_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryReports_ReportsId",
                table: "CategoryReports",
                column: "ReportsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryReports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ParentId",
                table: "Reports",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Categories_ParentId",
                table: "Reports",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
