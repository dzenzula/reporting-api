using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class LocationPoint
    {
        public LocationPoint()
        {
            WeightPoints = new HashSet<WeightPoint>();
        }

        public int Id { get; set; }
        public string NameLocation { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightPoint> WeightPoints { get; set; }
    }
}
