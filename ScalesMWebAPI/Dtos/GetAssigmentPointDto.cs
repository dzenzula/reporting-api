using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class GetAssigmentPointDto
    {
        public int Id { get; set; }
        public string NameAssigment { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime DtUpdate { get; set; }
        public String CreatedOn { get; set; }
        public String ModifyBy { get; set; }
    }
}
