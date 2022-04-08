using Microsoft.EntityFrameworkCore.Migrations;

namespace ScalesMWebAPI.Migrations
{
    public partial class deleteFkLogMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UWSScalesId",
                table: "LogErrorMessage",
                newName: "Weight_PointId");

            migrationBuilder.AddColumn<int>(
                name: "Weight_PLCId",
                table: "LogErrorMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight_PLCId",
                table: "LogErrorMessage");

            migrationBuilder.RenameColumn(
                name: "Weight_PointId",
                table: "LogErrorMessage",
                newName: "UWSScalesId");
        }
    }
}
