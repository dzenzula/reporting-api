using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScalesMWebAPI.Migrations
{
    public partial class AddCreateOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Weight_Sensors",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DtInsert",
                table: "Weight_Sensors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DtUpdate",
                table: "Weight_Sensors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Weight_Sensors",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Weight_Point",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Weight_Point",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Weight_PLC_Platform",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Weight_PLC_Platform",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Weight_PLC",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Weight_PLC",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Type_PLC",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Type_PLC",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "SensorCapture",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DtInsert",
                table: "SensorCapture",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DtUpdate",
                table: "SensorCapture",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "SensorCapture",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Location_Point",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Location_Point",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "Assigment_Point",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "Assigment_Point",
                type: "nvarchar(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Weight_Sensors");

            migrationBuilder.DropColumn(
                name: "DtInsert",
                table: "Weight_Sensors");

            migrationBuilder.DropColumn(
                name: "DtUpdate",
                table: "Weight_Sensors");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Weight_Sensors");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Weight_Point");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Weight_Point");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Weight_PLC_Platform");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Weight_PLC_Platform");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Weight_PLC");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Weight_PLC");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Type_PLC");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Type_PLC");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SensorCapture");

            migrationBuilder.DropColumn(
                name: "DtInsert",
                table: "SensorCapture");

            migrationBuilder.DropColumn(
                name: "DtUpdate",
                table: "SensorCapture");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "SensorCapture");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Location_Point");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Location_Point");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Assigment_Point");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Assigment_Point");
        }
    }
}
