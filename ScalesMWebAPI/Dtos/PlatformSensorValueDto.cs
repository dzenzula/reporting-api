using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class PlatformSensorValueDto
    {
        public int? Weight_PLCId { get; set; }
        public byte? PlatformN { get; set; }
        public Double? PlatformWeight { get; set; }
        public Double? FarRail { get; set; }
        public Double? NearRail { get; set; }
        public Double? LoadSensor1 { get; set; }
        public Double? LoadSensor2 { get; set; }
        public Double? LoadSensor3 { get; set; }
        public Double? LoadSensor4 { get; set; }
        public Boolean Stabilization { get; set; }
        public DateTime Dt { get; set; }
    }
}
