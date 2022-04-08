using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScalesMWebAPI.Migrations
{
    public partial class AddLogMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogErrorMessage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkWeightPlcPlatform = table.Column<int>(type: "int", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DtError = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogErrorMessage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogErrorMessage");
        }
    }
}
