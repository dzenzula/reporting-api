using Microsoft.EntityFrameworkCore.Migrations;

namespace ScalesMWebAPI.Migrations
{
    public partial class renameFkLogMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FkWeightPlcPlatform",
                table: "LogErrorMessage",
                newName: "UWSScalesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UWSScalesId",
                table: "LogErrorMessage",
                newName: "FkWeightPlcPlatform");
        }
    }
}
