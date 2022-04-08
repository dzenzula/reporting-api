﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScalesMWebAPI.Models;

namespace ScalesMWebAPI.Migrations
{
    [DbContext(typeof(KRRPAMONSCALESContext))]
    [Migration("20210901132429_updateLocationPoint")]
    partial class updateLocationPoint
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScalesMWebAPI.Dtos.TreeElementDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_Db")
                        .HasColumnType("int");

                    b.Property<bool>("Opened")
                        .HasColumnType("bit");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TreeElementDtoId")
                        .HasColumnType("int");

                    b.Property<int?>("TreeEquipmentDtoId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TreeElementDtoId");

                    b.HasIndex("TreeEquipmentDtoId");

                    b.ToTable("TreeElementDto");
                });

            modelBuilder.Entity("ScalesMWebAPI.Dtos.TreeEquipmentDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_Db")
                        .HasColumnType("int");

                    b.Property<bool>("Opened")
                        .HasColumnType("bit");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GetTreeDto");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.AssigmentPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameAssigment")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("name_assigment");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "NameAssigment" }, "IX_Assigment_Point")
                        .IsUnique();

                    b.ToTable("Assigment_Point");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.LocationPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime?>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameLocation")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("name_location");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "NameLocation" }, "IX_Location_Point")
                        .IsUnique();

                    b.ToTable("Location_Point");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.LogErrorMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtError")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("MessageText")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("MessageText");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WeightPlcid")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PLCId");

                    b.Property<int>("WeightPointId")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PointId");

                    b.HasKey("Id");

                    b.ToTable("LogErrorMessage");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.PerformedJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<DateTime>("DtWork")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_work");

                    b.Property<int>("ScalesNumberId")
                        .HasColumnType("int")
                        .HasColumnName("scales_numberId");

                    b.Property<int>("TypeWorkId")
                        .HasColumnType("int")
                        .HasColumnName("type_workId");

                    b.Property<int>("WeightPlcId")
                        .HasColumnType("int")
                        .HasColumnName("weight_plcId");

                    b.HasKey("Id");

                    b.HasIndex("WeightPlcId");

                    b.ToTable("Performed_Job");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.PlannedMaintenance", b =>
                {
                    b.Property<string>("Apr")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Aug")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Dec")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Feb")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Jan")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Jul")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Jun")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Mar")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("May")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Nov")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Oct")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<string>("Sep")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("nchar(1)")
                        .HasDefaultValueSql("((0))")
                        .IsFixedLength(true);

                    b.Property<int>("WeightPointId")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PointId");

                    b.Property<string>("YearDt")
                        .HasMaxLength(4)
                        .HasColumnType("nchar(4)")
                        .HasColumnName("Year_dt")
                        .IsFixedLength(true);

                    b.ToTable("Planned_Maintenance");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.SensorCapture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Dt")
                        .HasColumnType("datetime")
                        .HasColumnName("DT");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DtUtc")
                        .HasColumnType("datetime")
                        .HasColumnName("DT_UTC");

                    b.Property<double?>("FarRail")
                        .HasColumnType("float");

                    b.Property<byte?>("IgnoreCase")
                        .HasColumnType("tinyint")
                        .HasColumnName("ignore_case");

                    b.Property<double?>("LoadSensor1")
                        .HasColumnType("float")
                        .HasColumnName("load_sensor_1");

                    b.Property<double?>("LoadSensor2")
                        .HasColumnType("float")
                        .HasColumnName("load_sensor_2");

                    b.Property<double?>("LoadSensor3")
                        .HasColumnType("float")
                        .HasColumnName("load_sensor_3");

                    b.Property<double?>("LoadSensor4")
                        .HasColumnType("float")
                        .HasColumnName("load_sensor_4");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("NearRail")
                        .HasColumnType("float");

                    b.Property<byte>("PlatformN")
                        .HasColumnType("tinyint")
                        .HasColumnName("Platform_n");

                    b.Property<double?>("PlatformWeight")
                        .HasColumnType("float")
                        .HasColumnName("Platform_Weight");

                    b.Property<string>("Stabilization")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("WeightPlcid")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PLCId");

                    b.Property<int>("WeightPointId")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PointId");

                    b.HasKey("Id");

                    b.ToTable("SensorCapture");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.TypePlc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameType")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("name_type");

                    b.HasKey("Id");

                    b.ToTable("Type_PLC");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime?>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScaleNumberPlatform")
                        .HasColumnType("int")
                        .HasColumnName("scale_number_platform");

                    b.Property<int>("WeightPlcId")
                        .HasColumnType("int")
                        .HasColumnName("weight_plcId");

                    b.Property<int>("WeightPlcPlatform")
                        .HasColumnType("int")
                        .HasColumnName("weight_plc_platform");

                    b.Property<int>("WeightPointId")
                        .HasColumnType("int")
                        .HasColumnName("weight_pointId");

                    b.HasKey("Id");

                    b.HasIndex("WeightPlcId");

                    b.HasIndex("WeightPointId");

                    b.HasIndex(new[] { "ScaleNumberPlatform", "WeightPlcId", "WeightPlcPlatform" }, "UX_Weight_Platform")
                        .IsUnique();

                    b.ToTable("Weight_Platform");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NamePlc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name_plc");

                    b.Property<int>("ScalesNumberId")
                        .HasColumnType("int")
                        .HasColumnName("scales_numberId");

                    b.Property<string>("ServiceTag")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("service_tag")
                        .HasDefaultValueSql("((123456789))");

                    b.Property<int>("TypePlcId")
                        .HasColumnType("int")
                        .HasColumnName("type_plcId");

                    b.HasKey("Id");

                    b.HasIndex("ScalesNumberId");

                    b.HasIndex("TypePlcId");

                    b.HasIndex(new[] { "ServiceTag" }, "uind_Weigth_PLC")
                        .IsUnique();

                    b.ToTable("Weight_PLC");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlcPlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<int>("FkWeightPlc")
                        .HasColumnType("int")
                        .HasColumnName("FK_Weight_PLC");

                    b.Property<int>("IdScales")
                        .HasColumnType("int")
                        .HasColumnName("ID_Scales");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("WeightPlatform")
                        .HasColumnType("tinyint")
                        .HasColumnName("Weight_platform");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "IdScales", "FkWeightPlc", "WeightPlatform" }, "NonClusteredIndex-20210211-114816")
                        .IsUnique();

                    b.ToTable("Weight_PLC_Platform");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssigmentPointId")
                        .HasColumnType("int")
                        .HasColumnName("assigment_pointId");

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_insert");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime")
                        .HasColumnName("dt_update");

                    b.Property<int?>("FkExternalSystem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("fk_external_system")
                        .HasDefaultValueSql("((999))");

                    b.Property<int>("LocationPointId")
                        .HasColumnType("int")
                        .HasColumnName("location_pointId");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NamePoint")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name_point");

                    b.Property<string>("NumberScale")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("number_scale");

                    b.HasKey("Id");

                    b.HasIndex("AssigmentPointId");

                    b.HasIndex("LocationPointId");

                    b.HasIndex(new[] { "NumberScale" }, "IX_Weight_Point")
                        .IsUnique();

                    b.ToTable("Weight_Point");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightSensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtInsert")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DtInstall")
                        .HasColumnType("datetime")
                        .HasColumnName("Dt_install");

                    b.Property<DateTime>("DtUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceTag")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Service_Tag");

                    b.Property<int>("WeightPlcid")
                        .HasColumnType("int")
                        .HasColumnName("Weight_PLCId");

                    b.HasKey("Id");

                    b.HasIndex("WeightPlcid");

                    b.HasIndex(new[] { "ServiceTag" }, "IX_ServiceTag")
                        .IsUnique();

                    b.ToTable("Weight_Sensors");
                });

            modelBuilder.Entity("ScalesMWebAPI.Dtos.TreeElementDto", b =>
                {
                    b.HasOne("ScalesMWebAPI.Dtos.TreeElementDto", null)
                        .WithMany("Children")
                        .HasForeignKey("TreeElementDtoId");

                    b.HasOne("ScalesMWebAPI.Dtos.TreeEquipmentDto", null)
                        .WithMany("Children")
                        .HasForeignKey("TreeEquipmentDtoId");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.PerformedJob", b =>
                {
                    b.HasOne("ScalesMWebAPI.Models.WeightPlc", "WeightPlc")
                        .WithMany("PerformedJobs")
                        .HasForeignKey("WeightPlcId")
                        .HasConstraintName("FK_Performed_Job_Weight_PLC")
                        .IsRequired();

                    b.Navigation("WeightPlc");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlatform", b =>
                {
                    b.HasOne("ScalesMWebAPI.Models.WeightPlc", "WeightPlc")
                        .WithMany("WeightPlatforms")
                        .HasForeignKey("WeightPlcId")
                        .HasConstraintName("FK_Weight_Platform_Weight_PLC")
                        .IsRequired();

                    b.HasOne("ScalesMWebAPI.Models.WeightPoint", "WeightPoint")
                        .WithMany("WeightPlatforms")
                        .HasForeignKey("WeightPointId")
                        .HasConstraintName("FK_Weight_Platform_Weight_Point")
                        .IsRequired();

                    b.Navigation("WeightPlc");

                    b.Navigation("WeightPoint");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlc", b =>
                {
                    b.HasOne("ScalesMWebAPI.Models.WeightPoint", "ScalesNumber")
                        .WithMany("WeightPlcs")
                        .HasForeignKey("ScalesNumberId")
                        .HasConstraintName("FK_Weight_PLC_Weight_Point")
                        .IsRequired();

                    b.HasOne("ScalesMWebAPI.Models.TypePlc", "TypePlc")
                        .WithMany("WeightPlcs")
                        .HasForeignKey("TypePlcId")
                        .HasConstraintName("FK_Weigth_PLC_Weigth_Type_PLC")
                        .IsRequired();

                    b.Navigation("ScalesNumber");

                    b.Navigation("TypePlc");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPoint", b =>
                {
                    b.HasOne("ScalesMWebAPI.Models.AssigmentPoint", "AssigmentPoint")
                        .WithMany("WeightPoints")
                        .HasForeignKey("AssigmentPointId")
                        .HasConstraintName("FK_Weight_Point_Assigment_Point")
                        .IsRequired();

                    b.HasOne("ScalesMWebAPI.Models.LocationPoint", "LocationPoint")
                        .WithMany("WeightPoints")
                        .HasForeignKey("LocationPointId")
                        .HasConstraintName("FK_Weight_Point_Location_Point")
                        .IsRequired();

                    b.Navigation("AssigmentPoint");

                    b.Navigation("LocationPoint");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightSensor", b =>
                {
                    b.HasOne("ScalesMWebAPI.Models.WeightPlc", "WeightPlc")
                        .WithMany("WeightSensors")
                        .HasForeignKey("WeightPlcid")
                        .HasConstraintName("FK_Weight_Sensors_Weight_PLC")
                        .IsRequired();

                    b.Navigation("WeightPlc");
                });

            modelBuilder.Entity("ScalesMWebAPI.Dtos.TreeElementDto", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("ScalesMWebAPI.Dtos.TreeEquipmentDto", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.AssigmentPoint", b =>
                {
                    b.Navigation("WeightPoints");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.LocationPoint", b =>
                {
                    b.Navigation("WeightPoints");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.TypePlc", b =>
                {
                    b.Navigation("WeightPlcs");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPlc", b =>
                {
                    b.Navigation("PerformedJobs");

                    b.Navigation("WeightPlatforms");

                    b.Navigation("WeightSensors");
                });

            modelBuilder.Entity("ScalesMWebAPI.Models.WeightPoint", b =>
                {
                    b.Navigation("WeightPlatforms");

                    b.Navigation("WeightPlcs");
                });
#pragma warning restore 612, 618
        }
    }
}
