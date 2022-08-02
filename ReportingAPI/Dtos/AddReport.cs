using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class AddReport
    {
        [Required(ErrorMessage = "Нужно указать наименование элемента")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\.,|;\s\|\№\/\']+$",
              ErrorMessage = "Наименование элемента может содержать латиницу, кириллицу, цифры, пробел, символы: '-.,;/|№")]
        [MaxLength(150, ErrorMessage = "Наименование элемента не должно содержать больше 150 символов.")]
        public string Text { get; set; }

        [RegularExpression(@"[^\@\[\]\{\}\~\\\{\}\&]+",
            ErrorMessage = "Описание элемента НЕ может содержать фигурные скобки и символы: @,[],|,\\,~,&")]
        [MaxLength(150, ErrorMessage = "Описание не должно содержать больше 150 символов.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Не указан псевдоним")]
        [RegularExpression(@"[a-zA-Z0-9-]{4,50}",
            ErrorMessage = "Псевдоним может содержать только латиницу, цифры, дефис и содержать от 4 до 50 символов.")]
        public string Alias { get; set; }

        public int? ParentId { get; set; }
        public string URL { get; set; }
        public bool Visible { get; set; } = true;
    }
}
