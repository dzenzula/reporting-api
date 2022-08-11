using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public interface ITrackerChanges
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }

        /// <summary>
        /// Может вернуть Exception, если authenticatedUserName == null
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="authenticatedUserName"></param>
        public static void UpdateTrackerData(ref List<EntityEntry<ITrackerChanges>> entity, string authenticatedUserName)
        {
            if (authenticatedUserName is null)
                new NullReferenceException("authenticatedUserName не может быть пустым");

            var now = DateTime.Now;

            foreach (var entityEntry in entity)
            {
                if (entityEntry.Entity is not ITrackerChanges)
                    continue;

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property("UpdatedAt").CurrentValue = now;
                    entityEntry.Property("UpdatedBy").CurrentValue = authenticatedUserName;
                    entityEntry.Property("CreatedAt").IsModified = false;
                    entityEntry.Property("CreatedBy").IsModified = false;
                }

                else if (entityEntry.State == EntityState.Added)
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
