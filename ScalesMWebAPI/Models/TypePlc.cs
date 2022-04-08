using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class TypePlc
    {
        public TypePlc()
        {
            WeightPlcs = new HashSet<WeightPlc>();
        }

        public int Id { get; set; }
        public string NameType { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public virtual ICollection<WeightPlc> WeightPlcs { get; set; }
    }
}
