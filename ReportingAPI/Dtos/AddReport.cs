﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Dtos
{
    public class AddReport
    {
        [Required(ErrorMessage = "Нужно указать наименование элемента")]
        [RegularExpression(@"^[а-яА-ЯёЁЇїІіЄєҐґa-zA-Z0-9-\s]+$", ErrorMessage = "Наименование элемента может содержать только латиницу, кириллицу, цифры и дефис")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Не указан alias")]
        [RegularExpression(@"[a-zA-Z0-9-]{4,60}", ErrorMessage = "Alias может содержать только латиницу, цифры и дефис")]
        public string Alias { get; set; }
        public int? ParentId { get; set; }
        public string URL { get; set; }
        public bool Visible { get; set; } = true;
    }
}
