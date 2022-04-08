using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class UpdateWeightPointDto
    {
        public int Id { get; set; }
        public int LocationPointId { get; set; }
        public int AssigmentPointId { get; set; }
        public string NumberScale { get; set; }
        public string NamePoint { get; set; }
    }
}
