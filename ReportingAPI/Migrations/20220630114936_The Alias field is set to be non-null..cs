using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportingApi.Migrations
{
    public partial class TheAliasfieldissettobenonnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Alias",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Alias",
                table: "Reports",
                column: "Alias",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Alias",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Alias",
                table: "Reports",
                column: "Alias",
                unique: true,
                filter: "[Alias] IS NOT NULL");
        }
    }
}
