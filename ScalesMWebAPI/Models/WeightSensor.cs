using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightSensor
    {
        public int Id { get; set; }
        public int WeightPlcid { get; set; }
        public string ServiceTag { get; set; }
        public DateTime? DtInstall { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual WeightPlc WeightPlc { get; set; }
    }
}
