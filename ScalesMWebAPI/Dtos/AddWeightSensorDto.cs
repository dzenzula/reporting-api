using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class AddWeightSensorDto
    {
        [Required]
        public int WeightPlcid { get; set; }
        [Required]
        public string ServiceTag { get; set; }
        [Required]
        public DateTime? DtInstall { get; set; }
    }
}
