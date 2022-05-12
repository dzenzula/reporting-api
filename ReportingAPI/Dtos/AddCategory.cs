using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class AddCategory
    {
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
    }
}
