﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Report : ITreeItem
    {
        /* [Column("ReportId")]*/
        public int Id { get; set; }
        public string Text { get; set; }
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
    }
    public class Data
    {
        public string Url { get; set; }
        public Data(string URL) => Url = URL;
    }
}
