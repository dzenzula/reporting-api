using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightPlc
    {
        public WeightPlc()
        {
            PerformedJobs = new HashSet<PerformedJob>();
            WeightPlatforms = new HashSet<WeightPlatform>();
            WeightSensors = new HashSet<WeightSensor>();
        }

        public int Id { get; set; }
        public int ScalesNumberId { get; set; }
        public int TypePlcId { get; set; }
        public string NamePlc { get; set; }
        public string ServiceTag { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual WeightPoint ScalesNumber { get; set; }
        [JsonIgnore]
        public virtual TypePlc TypePlc { get; set; }
        [JsonIgnore]
        public virtual ICollection<PerformedJob> PerformedJobs { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightPlatform> WeightPlatforms { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightSensor> WeightSensors { get; set; }
    }
}
