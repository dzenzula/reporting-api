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
        DateTime UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
    }
}
