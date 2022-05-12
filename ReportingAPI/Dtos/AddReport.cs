using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class AddReport
    {
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
    }
}
