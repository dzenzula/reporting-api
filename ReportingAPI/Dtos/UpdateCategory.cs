using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class UpdateCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Нужно указать наименование элемента")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\.,|;\s\|\№\/\']+$",
            ErrorMessage = "Наименование элемента может содержать латиницу, кириллицу, цифры, пробел, символы: '-.,;/|№")]
        [MaxLength(150, ErrorMessage = "Наименование элемента не должно содержать больше 150 символов.")]
        public string Text { get; set; }

        [RegularExpression(@"[^\@\[\]\{\}\~\\\{\}\&]+",
            ErrorMessage = "Описание элемента НЕ может содержать фигурные скобки и символы: @,[],|,\\,~,&")]
        [MaxLength(150, ErrorMessage = "Описание не должно содержать больше 150 символов.")]
        public string Description { get; set; }

       // public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
    }
}
