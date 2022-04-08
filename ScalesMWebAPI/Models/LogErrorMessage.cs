using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Models
{
    [Table("LogErrorMessage", Schema = "dbo")]
    public class LogErrorMessage
    {
        [Key]
        public long Id { get; set; }
        public int WeightPointId { get; set; }
        public int WeightPlcid { get; set; }
        [MaxLength(500)]
        public string MessageText { get; set; }
        public DateTime DtError { get; set; }
        public DateTime DtInsert { get; set; }
        public DateTime DtUpdate { get; set; }
        public String CreatedOn { get; set; }
        public String ModifyBy { get; set; }
    }
}
