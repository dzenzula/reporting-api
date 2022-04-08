using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class GetUserNameDto
    {
        public string user { get; set; }
        public string domain { get; set; }
        public string client_host { get; set; }
        public DateTime time { get; set; }
    }
}
