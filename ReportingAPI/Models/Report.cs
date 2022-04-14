using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingAPI.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }

        public int? ParentId { get; set; }
       /* public Category Category { get; set; }*/
    }
}
