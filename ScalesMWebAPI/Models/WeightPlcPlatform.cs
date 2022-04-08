using System;
using System.Collections.Generic;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightPlcPlatform
    {
        public int Id { get; set; }
        public int IdScales { get; set; }
        public int FkWeightPlc { get; set; }
        public byte WeightPlatform { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
    }
}
