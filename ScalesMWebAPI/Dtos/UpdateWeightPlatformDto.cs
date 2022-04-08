using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class UpdateWeightPlatformDto
    {
        public int Id { get; set; }
        public int WeightPointId { get; set; }
        public int ScaleNumberPlatform { get; set; }
        public int WeightPlcId { get; set; }
        public int WeightPlcPlatform { get; set; }

    }
}
