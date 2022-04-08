using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace ScalesMWebAPI.Models
{
    public partial class WeightPointExtended
    {
        public WeightPointExtended()
        {

        }
        public WeightPointExtended(WeightPoint wp)
        {
            Id = wp.Id;
            LocationPointId = wp.LocationPointId;
            AssigmentPointId = wp.AssigmentPointId;
            FkExternalSystem = wp.FkExternalSystem;
            NumberScale = wp.NumberScale;
            NamePoint = wp.NamePoint;
            DtInsert = wp.DtInsert;
            DtUpdate = wp.DtUpdate;
        }
        public int Id { get; set; }
        public int LocationPointId { get; set; }
        public int AssigmentPointId { get; set; }
        public int? FkExternalSystem { get; set; }
        public string NumberScale { get; set; }
        public string NamePoint { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public WorkStatus Status { get; set; }
    }

    public enum WorkStatus
    {
        Debug = 0,
        Info = 1,
        Warn = 2, 
        Error = 3,
    }
}
