using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScalesMWebAPI.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assigment_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_assigment = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assigment_Point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Location_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location_Point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Planned_Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Weight_PointId = table.Column<int>(type: "int", nullable: false),
                    Jan = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Feb = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Mar = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Apr = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    May = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Jun = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Jul = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Aug = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Sep = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Oct = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Nov = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Dec = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "((0))"),
                    Year_dt = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SensorCapture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight_PointId = table.Column<int>(type: "int", nullable: false),
                    Weight_PLCId = table.Column<int>(type: "int", nullable: false),
                    Platform_n = table.Column<byte>(type: "tinyint", nullable: false),
                    load_sensor_1 = table.Column<double>(type: "float", nullable: true),
                    load_sensor_2 = table.Column<double>(type: "float", nullable: true),
                    load_sensor_3 = table.Column<double>(type: "float", nullable: true),
                    load_sensor_4 = table.Column<double>(type: "float", nullable: true),
                    Platform_Weight = table.Column<double>(type: "float", nullable: true),
                    FarRail = table.Column<double>(type: "float", nullable: true),
                    NearRail = table.Column<double>(type: "float", nullable: true),
                    Stabilization = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ignore_case = table.Column<byte>(type: "tinyint", nullable: true),
                    DT = table.Column<DateTime>(type: "datetime", nullable: false),
                    DT_UTC = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorCapture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_PLC",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_PLC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Weight_PLC_Platform",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Scales = table.Column<int>(type: "int", nullable: false),
                    FK_Weight_PLC = table.Column<int>(type: "int", nullable: false),
                    Weight_platform = table.Column<byte>(type: "tinyint", nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_PLC_Platform", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Weight_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location_pointId = table.Column<int>(type: "int", nullable: false),
                    assigment_pointId = table.Column<int>(type: "int", nullable: false),
                    fk_external_system = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((999))"),
                    number_scale = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name_point = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_Point", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weight_Point_Assigment_Point",
                        column: x => x.assigment_pointId,
                        principalTable: "Assigment_Point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Weight_Point_Location_Point",
                        column: x => x.location_pointId,
                        principalTable: "Location_Point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Weight_PLC",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scales_numberId = table.Column<int>(type: "int", nullable: false),
                    type_plcId = table.Column<int>(type: "int", nullable: false),
                    name_plc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    service_tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "((123456789))"),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_PLC", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weight_PLC_Weight_Point",
                        column: x => x.scales_numberId,
                        principalTable: "Weight_Point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Weigth_PLC_Weigth_Type_PLC",
                        column: x => x.type_plcId,
                        principalTable: "Type_PLC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Performed_Job",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scales_numberId = table.Column<int>(type: "int", nullable: false),
                    weight_plcId = table.Column<int>(type: "int", nullable: false),
                    type_workId = table.Column<int>(type: "int", nullable: false),
                    dt_work = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: false),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performed_Job", x => x.id);
                    table.ForeignKey(
                        name: "FK_Performed_Job_Weight_PLC",
                        column: x => x.weight_plcId,
                        principalTable: "Weight_PLC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Weight_Platform",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    weight_pointId = table.Column<int>(type: "int", nullable: false),
                    scale_number_platform = table.Column<int>(type: "int", nullable: false),
                    weight_plcId = table.Column<int>(type: "int", nullable: false),
                    weight_plc_platform = table.Column<int>(type: "int", nullable: false),
                    dt_insert = table.Column<DateTime>(type: "datetime", nullable: true),
                    dt_update = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_Platform", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weight_Platform_Weight_PLC",
                        column: x => x.weight_plcId,
                        principalTable: "Weight_PLC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Weight_Platform_Weight_Point",
                        column: x => x.weight_pointId,
                        principalTable: "Weight_Point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Weight_Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight_PLCId = table.Column<int>(type: "int", nullable: false),
                    Service_Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dt_install = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weight_Sensors_Weight_PLC",
                        column: x => x.Weight_PLCId,
                        principalTable: "Weight_PLC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assigment_Point",
                table: "Assigment_Point",
                column: "name_assigment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_Point",
                table: "Location_Point",
                column: "name_location",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Performed_Job_weight_plcId",
                table: "Performed_Job",
                column: "weight_plcId");

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Platform_weight_plcId",
                table: "Weight_Platform",
                column: "weight_plcId");

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Platform_weight_pointId",
                table: "Weight_Platform",
                column: "weight_pointId");

            migrationBuilder.CreateIndex(
                name: "UX_Weight_Platform",
                table: "Weight_Platform",
                columns: new[] { "scale_number_platform", "weight_plcId", "weight_plc_platform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_PLC_scales_numberId",
                table: "Weight_PLC",
                column: "scales_numberId");

            migrationBuilder.CreateIndex(
                name: "IX_Weight_PLC_type_plcId",
                table: "Weight_PLC",
                column: "type_plcId");

            migrationBuilder.CreateIndex(
                name: "uind_Weigth_PLC",
                table: "Weight_PLC",
                column: "service_tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NonClusteredIndex-20210211-114816",
                table: "Weight_PLC_Platform",
                columns: new[] { "ID_Scales", "FK_Weight_PLC", "Weight_platform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Point",
                table: "Weight_Point",
                column: "number_scale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Point_assigment_pointId",
                table: "Weight_Point",
                column: "assigment_pointId");

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Point_location_pointId",
                table: "Weight_Point",
                column: "location_pointId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTag",
                table: "Weight_Sensors",
                column: "Service_Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Sensors_Weight_PLCId",
                table: "Weight_Sensors",
                column: "Weight_PLCId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performed_Job");

            migrationBuilder.DropTable(
                name: "Planned_Maintenance");

            migrationBuilder.DropTable(
                name: "SensorCapture");

            migrationBuilder.DropTable(
                name: "Weight_Platform");

            migrationBuilder.DropTable(
                name: "Weight_PLC_Platform");

            migrationBuilder.DropTable(
                name: "Weight_Sensors");

            migrationBuilder.DropTable(
                name: "Weight_PLC");

            migrationBuilder.DropTable(
                name: "Weight_Point");

            migrationBuilder.DropTable(
                name: "Type_PLC");

            migrationBuilder.DropTable(
                name: "Assigment_Point");

            migrationBuilder.DropTable(
                name: "Location_Point");
        }
    }
}
