using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class ReportingContext : DbContext
    {
        /*private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportingApiContext(DbContextOptions<ReportingApiContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }*/

        public ReportingContext(DbContextOptions<ReportingContext> options) : base(options)
        {
           
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*modelBuilder.Entity<Category>()
                      .HasMany(j => j.Reports)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);*/

            modelBuilder.Entity<Category>()
                      .HasMany(j => j.Categories)
                      .WithOne(j => j.Parent)
                      .HasForeignKey(j => j.ParentId);

        }


    }
}
