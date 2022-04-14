using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        
        /*public List<Report> Report { get; set; }*/
    }
}
