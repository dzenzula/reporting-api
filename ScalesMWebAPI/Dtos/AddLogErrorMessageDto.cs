using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class AddLogErrorMessageDto
    {
        public int UWSScalesId { get; set; }
        [MaxLength(500)]
        public string MessageText { get; set; }
    }
}
