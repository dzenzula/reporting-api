using System;

namespace ScalesMWebAPI.Dtos
{
    public class WeightSensorDto
    {
        public int id { get; set; }
        public string ServiceTag { get; set; }
        public DateTime? DtInstall { get; set; }
    }
}