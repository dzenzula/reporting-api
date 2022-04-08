using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class UpdateWeightSensorDataDto
    {
        public int WeightPlcid { get; set; }
        public string OldServiceTag { get; set; }
        public string NewServiceTag { get; set; }
        public DateTime? DtWork { get; set; }
    }
}
