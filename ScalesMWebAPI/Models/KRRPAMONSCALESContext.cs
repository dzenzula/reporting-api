using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ScalesMWebAPI.Services.Handlers;
using ScalesMWebAPI.Dtos;
using Microsoft.Extensions.Configuration;


#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class KRRPAMONSCALESContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //protected readonly IConfiguration _configuration;
        public KRRPAMONSCALESContext()
        {
        }
        //public KRRPAMONSCALESContext(IConfiguration configuration) 
        //{
        //}
        public KRRPAMONSCALESContext(DbContextOptions<KRRPAMONSCALESContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public virtual DbSet<AssigmentPoint> AssigmentPoints { get; set; }
        public virtual DbSet<LocationPoint> LocationPoints { get; set; }
        public virtual DbSet<PerformedJob> PerformedJobs { get; set; }
        public virtual DbSet<PlannedMaintenance> PlannedMaintenances { get; set; }
        public virtual DbSet<SensorCapture> SensorCaptures { get; set; }
        public virtual DbSet<TypePlc> TypePlcs { get; set; }
        public virtual DbSet<WeightPlatform> WeightPlatforms { get; set; }
        public virtual DbSet<WeightPlc> WeightPlcs { get; set; }
        public virtual DbSet<WeightPlcPlatform> WeightPlcPlatforms { get; set; }
        public virtual DbSet<WeightPoint> WeightPoints { get; set; }
        public virtual DbSet<WeightSensor> WeightSensors { get; set; }
        public virtual DbSet<LogErrorMessage> LogErrorMessages { get; set; }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(true, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string authenticatedUserName = "";
            string authenticatedUserDomain = "";
            if (httpContext != null)
            {
                Security.GetUserNameDomain(httpContext.User,out authenticatedUserName, out authenticatedUserDomain);

                // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                //var authenticatedUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst("sub").Value

                // TODO use name to set the shadow property value like in the following post: https://www.meziantou.net/2017/07/03/entity-framework-core-generate-tracking-columns
            }
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                bool audited_dt_insert = false;
                bool audited_insert = false;
                bool audited_update = false;
                bool audited_dt_update = false;
                foreach (var item in entityEntry.Members)
                {
                    if (audited_dt_insert == false) audited_dt_insert = item.Metadata.PropertyInfo.Name == "DtInsert" ? true : false;
                    if (audited_insert == false) audited_insert = item.Metadata.PropertyInfo.Name == "CreatedOn" ? true : false;
                    if (audited_update == false) audited_update = item.Metadata.PropertyInfo.Name == "ModifyBy" ? true : false;
                    if (audited_dt_update == false) audited_dt_update = item.Metadata.PropertyInfo.Name == "DtUpdate" ? true : false;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    if (audited_dt_update) entityEntry.Property("DtUpdate").CurrentValue = DateTime.Now;
                    if (audited_dt_insert) entityEntry.Property("DtInsert").IsModified = false;
                    if (audited_update) entityEntry.Property("ModifyBy").CurrentValue = authenticatedUserName;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    if (audited_dt_insert) { entityEntry.Property("DtInsert").CurrentValue = DateTime.Now; }
                    if (audited_dt_update) entityEntry.Property("DtUpdate").IsModified = false;
                    if (audited_insert) entityEntry.Property("CreatedOn").CurrentValue = authenticatedUserName;
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=KRR-TST-PAHWL02;Database=KRR-PA-MON-SCALES;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var allEntities = modelBuilder.Model.GetEntityTypes();
            modelBuilder.HasDefaultSchema("dbo");
            //foreach (var entity in allEntities)
            //{
            //    entity.AddProperty("CreatedOn", typeof(String));
            //    entity.AddProperty("ModifyBy", typeof(String));
            //}

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AssigmentPoint>(entity =>
            {
                entity.ToTable("Assigment_Point");

                entity.HasIndex(e => e.NameAssigment, "IX_Assigment_Point")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.NameAssigment)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("name_assigment");
            });
            modelBuilder.Entity<LogErrorMessage>(entity => {
                entity.ToTable("LogErrorMessage");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.WeightPlcid).HasColumnName("Weight_PLCId");
                entity.Property(e => e.WeightPointId).HasColumnName("Weight_PointId");
                entity.Property(e => e.MessageText).HasColumnName("MessageText").HasMaxLength(500);
                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");
            }
                );
            modelBuilder.Entity<LocationPoint>(entity =>
            {
                entity.ToTable("Location_Point");

                entity.HasIndex(e => e.NameLocation, "IX_Location_Point")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.NameLocation)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name_location");
            });

            modelBuilder.Entity<PerformedJob>(entity =>
            {
                entity.ToTable("Performed_Job");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.DtWork)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_work");

                entity.Property(e => e.ScalesNumberId).HasColumnName("scales_numberId");

                entity.Property(e => e.TypeWorkId).HasColumnName("type_workId");

                entity.Property(e => e.WeightPlcId).HasColumnName("weight_plcId");

                entity.HasOne(d => d.WeightPlc)
                    .WithMany(p => p.PerformedJobs)
                    .HasForeignKey(d => d.WeightPlcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performed_Job_Weight_PLC");
            });

            modelBuilder.Entity<PlannedMaintenance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Planned_Maintenance");

                entity.Property(e => e.Apr)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Aug)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Dec)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Feb)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Jan)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Jul)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Jun)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Mar)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.May)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Nov)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Oct)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.Sep)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength(true);

                entity.Property(e => e.WeightPointId).HasColumnName("Weight_PointId");

                entity.Property(e => e.YearDt)
                    .HasMaxLength(4)
                    .HasColumnName("Year_dt")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SensorCapture>(entity =>
            {
                entity.ToTable("SensorCapture");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("DT");

                entity.Property(e => e.DtUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_UTC");

                entity.Property(e => e.IgnoreCase).HasColumnName("ignore_case");

                entity.Property(e => e.LoadSensor1).HasColumnName("load_sensor_1");

                entity.Property(e => e.LoadSensor2).HasColumnName("load_sensor_2");

                entity.Property(e => e.LoadSensor3).HasColumnName("load_sensor_3");

                entity.Property(e => e.LoadSensor4).HasColumnName("load_sensor_4");

                entity.Property(e => e.PlatformN).HasColumnName("Platform_n");

                entity.Property(e => e.PlatformWeight).HasColumnName("Platform_Weight");

                entity.Property(e => e.Stabilization).HasMaxLength(5);

                entity.Property(e => e.WeightPlcid).HasColumnName("Weight_PLCId");

                entity.Property(e => e.WeightPointId).HasColumnName("Weight_PointId");
            });

            modelBuilder.Entity<TypePlc>(entity =>
            {
                entity.ToTable("Type_PLC");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.NameType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("name_type");
            });

            modelBuilder.Entity<WeightPlatform>(entity =>
            {
                entity.ToTable("Weight_Platform");

                entity.HasIndex(e => new { e.ScaleNumberPlatform, e.WeightPlcId, e.WeightPlcPlatform }, "UX_Weight_Platform")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.ScaleNumberPlatform).HasColumnName("scale_number_platform");

                entity.Property(e => e.WeightPlcId).HasColumnName("weight_plcId");

                entity.Property(e => e.WeightPlcPlatform).HasColumnName("weight_plc_platform");

                entity.Property(e => e.WeightPointId).HasColumnName("weight_pointId");

                entity.HasOne(d => d.WeightPlc)
                    .WithMany(p => p.WeightPlatforms)
                    .HasForeignKey(d => d.WeightPlcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Platform_Weight_PLC");

                entity.HasOne(d => d.WeightPoint)
                    .WithMany(p => p.WeightPlatforms)
                    .HasForeignKey(d => d.WeightPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Platform_Weight_Point");
            });

            modelBuilder.Entity<WeightPlc>(entity =>
            {
                entity.ToTable("Weight_PLC");

                entity.HasIndex(e => e.ServiceTag, "uind_Weigth_PLC")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.NamePlc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name_plc");

                entity.Property(e => e.ScalesNumberId).HasColumnName("scales_numberId");

                entity.Property(e => e.ServiceTag)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("service_tag")
                    .HasDefaultValueSql("((123456789))");

                entity.Property(e => e.TypePlcId).HasColumnName("type_plcId");

                entity.HasOne(d => d.ScalesNumber)
                    .WithMany(p => p.WeightPlcs)
                    .HasForeignKey(d => d.ScalesNumberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_PLC_Weight_Point");

                entity.HasOne(d => d.TypePlc)
                    .WithMany(p => p.WeightPlcs)
                    .HasForeignKey(d => d.TypePlcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weigth_PLC_Weigth_Type_PLC");
            });

            modelBuilder.Entity<WeightPlcPlatform>(entity =>
            {
                entity.ToTable("Weight_PLC_Platform");

                entity.HasIndex(e => new { e.IdScales, e.FkWeightPlc, e.WeightPlatform }, "NonClusteredIndex-20210211-114816")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.FkWeightPlc).HasColumnName("FK_Weight_PLC");

                entity.Property(e => e.IdScales).HasColumnName("ID_Scales");

                entity.Property(e => e.WeightPlatform).HasColumnName("Weight_platform");
            });

            modelBuilder.Entity<WeightPoint>(entity =>
            {
                entity.ToTable("Weight_Point");

                entity.HasIndex(e => e.NumberScale, "IX_Weight_Point")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AssigmentPointId).HasColumnName("assigment_pointId");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_insert");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_update");

                entity.Property(e => e.FkExternalSystem)
                    .HasColumnName("fk_external_system")
                    .HasDefaultValueSql("((999))");

                entity.Property(e => e.LocationPointId).HasColumnName("location_pointId");

                entity.Property(e => e.NamePoint)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name_point");

                entity.Property(e => e.NumberScale)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("number_scale");

                entity.HasOne(d => d.AssigmentPoint)
                    .WithMany(p => p.WeightPoints)
                    .HasForeignKey(d => d.AssigmentPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Point_Assigment_Point");

                entity.HasOne(d => d.LocationPoint)
                    .WithMany(p => p.WeightPoints)
                    .HasForeignKey(d => d.LocationPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Point_Location_Point");
            });

            modelBuilder.Entity<WeightSensor>(entity =>
            {
                entity.ToTable("Weight_Sensors");
                entity.HasIndex(e => e.ServiceTag, "IX_ServiceTag")
                    .IsUnique();
                entity.Property(e => e.DtInstall)
                    .HasColumnType("datetime")
                    .HasColumnName("Dt_install");

                entity.Property(e => e.ServiceTag)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Service_Tag");

                entity.Property(e => e.WeightPlcid).HasColumnName("Weight_PLCId");

                entity.HasOne(d => d.WeightPlc)
                    .WithMany(p => p.WeightSensors)
                    .HasForeignKey(d => d.WeightPlcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Sensors_Weight_PLC");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        public DbSet<ScalesMWebAPI.Dtos.TreeEquipmentDto> GetTreeDto { get; set; }

    }
}
