using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ReportingApi.Models
{
    public interface ITreeItem
    {
        public int Id { get; }
        public int? ParentId { get; }
        public string Text { get; }
        public string Type { get; }
        public Data Data { get; }
        public ICollection<ITreeItem> Children { get; }
    }
    /*public class Data
    {
        public string Url { get; set; }
        public Data(string URL) => Url = URL;
    }*/
}