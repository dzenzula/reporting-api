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

        [Required(ErrorMessage = "Нужно указать наименование элемента!")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\.,|;\s\|\№\/\']+$",
            ErrorMessage = "Наименование элемента может содержать латиницу, кириллицу, цифры, пробел, символы: '-.,;/|№")]
        [MaxLength(150, ErrorMessage = "Наименование элемента не должно содержать больше 150 символов.")]
        public string Text { get; set; }

        [RegularExpression(@"[^\@\[\]\{\}\~\\\{\}\&]+",
            ErrorMessage = "Описание элемента НЕ может содержать фигурные скобки и символы: @,[],|,\\,~,&")]
        [MaxLength(150, ErrorMessage = "Описание не должно содержать больше 150 символов.")]
        public string Description { get; set; } = "default";

        [Required(ErrorMessage ="Нужно указать владельца отчета!")]
        public string Owner { get; set; }
        public bool Visible { get; set; } = true;
        public string URL { get; set; }
        [Required(ErrorMessage = "Не указана группа отчета")]
        [RegularExpression(@"[a-z0-9_]{3,40}",
            ErrorMessage = "Псевдоним может содержать только латиницу, цифры, дефис и содержать от 3 до 40 символов.")]
        public string Operation_name { get; set; }
    }
}
