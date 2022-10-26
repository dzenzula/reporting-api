using amkr.csharp_common_libs.Security;
using amkr.csharp_common_libs.TrackerChanges;
using AuthorizationApiHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public ReportingContext(DbContextOptions<ReportingContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<FavoriteReport> FavoriteReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Category>()
                      .HasMany(j => j.Reports)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);*/

            /*modelBuilder.Entity<Category>()
                      .HasMany(j => j.Categories)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);*/
            modelBuilder.Entity<Category>()
                      .HasMany(j => j.Reports)
                      .WithMany(j => j.Categories)
                      .UsingEntity(j => j.ToTable("CategoryReports"));
            modelBuilder.Entity<Category>()
                .HasMany(k => k.Categories)
                .WithOne(k => k.Parent)
                .HasForeignKey(k => k.ParentId);

            /*modelBuilder.Entity<Report>()
                .HasMany(l => l.Categories)
                .WithMany(l => l.Reports);*/


            modelBuilder.Entity<Report>()
                .HasIndex(c => c.Text)
                .IsUnique();

            modelBuilder.Entity<Report>()
                .HasIndex(c => c.Alias)
                .IsUnique();

            modelBuilder.Entity<Report>()
                .HasIndex(c => c.URL)
                .IsUnique();

            modelBuilder.Entity<FavoriteReport>()
                .HasIndex(c => new { c.Login, c.ReportId })
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
            var entries = ChangeTracker
                .Entries<ITrackerChanges>()
                .Where(e =>
                        (e.State == EntityState.Added
                        || e.State == EntityState.Modified)).ToList();

            if (entries.Count > 0)
            {
                AuthUserData userData = new AuthUserData();

                userData.DisplayName = _httpContextAccessor.HttpContext.Session.GetString("USER_NAME");
                userData.Name = _httpContextAccessor.HttpContext.Session.GetString("USER_DOMAIN_NAME");
                userData.Type = _httpContextAccessor.HttpContext.Session.GetString("USER_TYPE");
                userData.Domain = _httpContextAccessor.HttpContext.Session.GetString("USER_DOMAIN");
                userData.AuthStatus = _httpContextAccessor.HttpContext.Session.GetInt32("USER_IS_AUTH");
                ITrackerChanges.UpdateTrackerData(ref entries, userData);
            }
        }
    }
}
