using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Category : ITrackerChanges
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        [NotMapped]
        public string Type { get => "folder"; }
        [NotMapped]
        public Data Data { get => null; }
       // [JsonIgnore]
        public int? ParentId { get; set; }
        public bool Visible { get; set; } 
        [NotMapped]
        [JsonIgnore]
        public virtual Category Parent { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }

        /*[NotMapped]
        [JsonIgnore]
        public ICollection<ITreeItem> Children
        {
            get
            {
                var tree = new List<ITreeItem>();
                if (Categories != null)
                {
                    tree.AddRange(Categories);
                }
                if (Reports != null)
                {
                    tree.AddRange(Reports);
                }
                return tree;
            }
        }*/

        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }

        /*[JsonIgnore]*/
        public ICollection<Report> Reports { get; set; }
        /*[JsonIgnore]
        public ICollection<Report> Reports { get; set; }*/

        public Category()
        {
            Categories = new List<Category>();
            Reports = new List<Report>();
        }
    }
}
