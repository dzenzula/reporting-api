using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class GetWeightSensorDto
    {
        [Required]
        public int WeightPlcid { get; set; }
        public List<WeightSensorDto> Sensors { get; set; }
    }
}
