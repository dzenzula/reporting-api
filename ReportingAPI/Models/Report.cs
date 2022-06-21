using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Report : ITreeItem, ITrackerChanges
    {
        /* [Column("ReportId")]*/
        public int Id { get; set; }
        // атрибут Unique для Alias задан через modelBuilder в ReportingContext
        public string Alias { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string Type { get => "file"; }
       // [JsonIgnore]
        public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
        [NotMapped]
        [JsonIgnore]
        public virtual Category Parent { get; set; }
        [JsonIgnore]
        public string URL { get; private set; }
        [NotMapped]
        public Data Data { get => new Data(URL); }

        public ICollection<ITreeItem> Children { get => null; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
    }
    public class Data
    {
        public string Url { get; set; }
        public Data(string URL) => Url = URL;
    }
}
