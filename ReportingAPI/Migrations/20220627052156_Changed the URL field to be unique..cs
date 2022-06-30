using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class ChangedtheURLfieldtobeunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_URL",
                table: "Reports",
                column: "URL",
                unique: true,
                filter: "[URL] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_URL",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
