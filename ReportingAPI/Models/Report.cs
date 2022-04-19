using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportingApi.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public string URL { get; private set; }

        /*public int? ParentId { get; set; }*/

        [NotMapped]
        public Data data { get => new Data(URL);}

        public int ParentId { get; set; }
        [JsonIgnore]
        public Category Parent { get; set; }

        /*public object dataT
        {
            get
            {
                dynamic data = new System.Dynamic.ExpandoObject();
                data.url = URL;
                return data;
            }
        }*/
    }
    public class Data
    {
        public string Url { get; set; }
        public Data(string URL) => Url = URL;
    }
}
