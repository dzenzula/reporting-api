using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ReportingApi.Models
{
   // [Index(nameof(Login), nameof(Report), IsUnique = true)]
    public class FavoriteReport
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        //  [Index("IX_FirstAndSecond", 1, IsUnique = true)]
        [JsonIgnore]
        public string Login { get; set; }
        [Required]
        [JsonIgnore]
        public int ReportId { get; set; }
        //[JsonIgnore]
        public Report Report { get; set; }


    }
}
