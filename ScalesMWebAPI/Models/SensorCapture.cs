using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class SensorCapture
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int WeightPointId { get; set; }
        public int WeightPlcid { get; set; }
        public byte PlatformN { get; set; }
        public double? LoadSensor1 { get; set; }
        public double? LoadSensor2 { get; set; }
        public double? LoadSensor3 { get; set; }
        public double? LoadSensor4 { get; set; }
        public double? PlatformWeight { get; set; }
        public double? FarRail { get; set; }
        public double? NearRail { get; set; }
        public string Stabilization { get; set; }
        [JsonIgnore]
        public byte? IgnoreCase { get; set; }
        public DateTime Dt { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string CreatedOn { get; set; }
        public string ModifyBy { get; set; }
        [JsonIgnore]
        public DateTime? DtUtc { get; set; }
    }
}
