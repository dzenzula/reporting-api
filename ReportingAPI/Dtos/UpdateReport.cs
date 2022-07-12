using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class UpdateReport
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Нужно указать наименование элемента")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\s\|\.]+$", 
            ErrorMessage = "Наименование элемента может содержать: латиницу, кириллицу, цифры, дефис, пробел, точку, \"|\"")]
        public string Text { get; set; }
        // 150 symbols
        [MaxLength(150, ErrorMessage = "Описание не может содержать больше 150 символов.")]
        public string Description { get; set; } = "default";
        /*[Required(ErrorMessage = "Не указан alias")]
        [RegularExpression(@"[a-zA-Z0-9-]{4,60}", ErrorMessage = "Alias может содержать только латиницу, цифры и дефис")]
        public string Alias { get; set; }
        public int? ParentId { get; set; }*/
        public bool Visible { get; set; } = true;
        public string URL { get; set; }
    }
}
