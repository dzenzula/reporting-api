using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class GetSensorValueDto
    {
        [Required]
        public int Weight_PointId { get; set; }
        public List<PlatformSensorValueDto> Platforms { get; set; }
    }
}
