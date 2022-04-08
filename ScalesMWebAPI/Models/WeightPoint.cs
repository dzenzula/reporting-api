using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightPoint
    {
        public WeightPoint()
        {
            WeightPlatforms = new HashSet<WeightPlatform>();
            WeightPlcs = new HashSet<WeightPlc>();
        }

        public int Id { get; set; }
        public int LocationPointId { get; set; }
        public int AssigmentPointId { get; set; }
        public int? FkExternalSystem { get; set; }
        public string NumberScale { get; set; }
        public string NamePoint { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual AssigmentPoint AssigmentPoint { get; set; }
        [JsonIgnore]
        public virtual LocationPoint LocationPoint { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightPlatform> WeightPlatforms { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightPlc> WeightPlcs { get; set; }
    }
}
