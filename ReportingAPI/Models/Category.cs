using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public virtual Category Parent { get; set; }
/*        [JsonIgnore]
        public virtual Report item { get; set; }  */ 

        public ICollection<Category> Children { get; set; }
        
        /*public List<Report> Report { get; set; }*/
    }
}
