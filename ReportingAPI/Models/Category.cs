using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Category : BaseTreeItem
    {

        [NotMapped]
        public ICollection<BaseTreeItem> Children { get {
                var tree = new List<BaseTreeItem>();
                tree.AddRange(Categories);
                tree.AddRange(Reports);
                return tree;
            } 
        }
        /*        [JsonIgnore]
                public virtual Report item { get; set; }  */
        public virtual Category Parent { get; set; }
        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }
        [JsonIgnore]
        public ICollection<Report> Reports { get; set; }

        /*public List<Report> Report { get; set; }*/
    }
}
