using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReportingApi.Services.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class ReportingContext : DbContext
    {  
        private readonly IHttpContextAccessor _httpContextAccessor;

       /* public ReportingApiContext(DbContextOptions<ReportingApiContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }*/

        public ReportingContext(DbContextOptions<ReportingContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
                      .HasMany(j => j.Reports)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);

            modelBuilder.Entity<Category>()
                      .HasMany(j => j.Categories)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);

            modelBuilder.Entity<Report>()
                .HasIndex(c => c.Alias)
                .IsUnique();

        }

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

        public void OnBeforeSaving()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string authenticatedUserName = "";
            string authenticatedUserDomain = "";
            if (httpContext != null)
            {
                Security.GetUserNameDomain(httpContext.User, out authenticatedUserName, out authenticatedUserDomain);

                // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                //var authenticatedUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst("sub").Value

                // TODO use name to set the shadow property value like in the following post: https://www.meziantou.net/2017/07/03/entity-framework-core-generate-tracking-columns
            }

            var now = DateTime.Now;

            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                /*bool audited_dt_insert = false;
                bool audited_insert = false;
                bool audited_update = false;
                bool audited_dt_update = false;
                foreach (var item in entityEntry.Members)
                {
                    if (audited_dt_insert == false) audited_dt_insert = item.Metadata.PropertyInfo.Name == "DtInsert" ? true : false;
                    if (audited_insert == false) audited_insert = item.Metadata.PropertyInfo.Name == "CreatedOn" ? true : false;
                    if (audited_update == false) audited_update = item.Metadata.PropertyInfo.Name == "ModifyBy" ? true : false;
                    if (audited_dt_update == false) audited_dt_update = item.Metadata.PropertyInfo.Name == "DtUpdate" ? true : false;
                }*/
                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property("UpdatedAt").CurrentValue = now;
                    entityEntry.Property("UpdatedBy").CurrentValue = authenticatedUserName;
                    entityEntry.Property("CreatedAt").IsModified = false;
                    entityEntry.Property("CreatedBy").IsModified = false;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("UpdatedAt").IsModified = false;
                    entityEntry.Property("UpdatedBy").IsModified = false;
                    entityEntry.Property("CreatedAt").CurrentValue = now;
                    entityEntry.Property("CreatedBy").CurrentValue = authenticatedUserName;
                }
            }

        }

    }
}
