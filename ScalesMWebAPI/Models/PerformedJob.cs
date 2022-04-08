using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class PerformedJob
    {
        public int Id { get; set; }
        public int ScalesNumberId { get; set; }
        public int WeightPlcId { get; set; }
        public int TypeWorkId { get; set; }
        public DateTime DtWork { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime DtUpdate { get; set; }
        [JsonIgnore]
        public virtual WeightPlc WeightPlc { get; set; }
    }
}
