using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class UpdateWeightPlcDto
    {
        public int Id { get; set; }
        public int ScalesNumberId { get; set; }
        public int TypePlcId { get; set; }
        public string NamePlc { get; set; }
        public string ServiceTag { get; set; }
    }
}
