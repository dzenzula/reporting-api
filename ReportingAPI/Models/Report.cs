using amkr.csharp_common_libs.TrackerChanges;
using AuthorizationApiHandler.Models;
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
    public class Report : ITrackerChanges
    {
        [NotMapped]
        [JsonIgnore]
        public ICollection<FavoriteReport> FavoriteReports { get; set; }
        /* [Column("ReportId")]*/
        public int Id { get; set; }
        // атрибут Unique для Alias задан через modelBuilder в ReportingContext
        [MaxLength(50)]
        [Required]
        public string Alias { get; set; }
        [Required]
        [MaxLength(150)]
        public string Text { get; set; }
        //, ErrorMessage = "Описание не может содержать больше 150 символов.")
        [MaxLength(150)]
        public string Description { get; set; }
        [NotMapped]
        public string Type { get => "file"; }
        // public int? ParentId { get; set; }
        [NotMapped]
        public int ParentId { get; set; }
        public bool Visible { get; set; } = true;
        [NotMapped]
        [JsonIgnore]
        public virtual Category Parent { get; set; }
        [Required]
        [MaxLength(1000)]
        [JsonIgnore]
        public string URL { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Operation_name { get; set; }
        [NotMapped]
        public Data Data { get => new Data(URL); }

        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }

        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }

        

        public Report()
        {
            Categories = new List<Category>();
        }

        public Report Clone()
        {
            return (Report)this.MemberwiseClone();
        }
    }
}
    public class Data
    {
        public string Url { get; set; }
        public Data(string URL) => Url = URL;
    }
    
