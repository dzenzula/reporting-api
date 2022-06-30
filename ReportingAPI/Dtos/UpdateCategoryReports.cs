using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class UpdateCategoryReports
    {
        public int id { get; set; }
        public int fromCat { get; set; }
        public int toCat { get; set; }
    }
}
