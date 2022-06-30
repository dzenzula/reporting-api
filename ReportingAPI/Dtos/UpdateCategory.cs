﻿using System;
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
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\s\|\.]+$", 
            ErrorMessage = "Наименование элемента может содержать: латиницу, кириллицу, цифры, дефис, пробел, точку, \"|\"")]
        public string Text { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }
        /*[Required(ErrorMessage = "Нужно указать описание элемента")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\s]+$", ErrorMessage = "Наименование элемента может содержать только латиницу, кириллицу, цифры и дефис")]
        public string Description { get; set; }*/
        public int? ParentId { get; set; }
        public bool Visible { get; set; } = true;
    }
}
