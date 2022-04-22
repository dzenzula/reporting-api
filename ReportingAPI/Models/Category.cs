﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Category 
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [NotMapped]
        public string Type { get => "folder"; }
        [NotMapped]
        public Data Data { get => null; }
       // [JsonIgnore]
        public int? ParentId { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual Category Parent { get; set; }

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
        /*[JsonIgnore]
        public ICollection<Report> Reports { get; set; }*/
    }
}
