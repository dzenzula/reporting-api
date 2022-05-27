using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class UpdateReport
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Alias { get; set; }
        public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
        public string URL { get; set; }
    }
}
