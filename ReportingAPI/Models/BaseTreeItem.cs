using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ReportingApi.Models
{
    public abstract class BaseTreeItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }

        /*[NotMapped]
        public virtual Category Parent { get; set; } */  
    }
}