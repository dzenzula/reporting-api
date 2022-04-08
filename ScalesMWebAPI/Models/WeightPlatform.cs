using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightPlatform
    {
        public int Id { get; set; }
        public int WeightPointId { get; set; }
        public int ScaleNumberPlatform { get; set; }
        public int WeightPlcId { get; set; }
        public int WeightPlcPlatform { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual WeightPlc WeightPlc { get; set; }
        [JsonIgnore]
        public virtual WeightPoint WeightPoint { get; set; }
    }
}
