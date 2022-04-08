using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.Dtos
{
    public class TreeEquipmentDto
    {
        /*Index tree*/
        public int Id { set; get; }
        public int Id_Db { set; get; }
        public int Type { set; get; }
        public string Text { set; get; }
        public bool Opened { set; get; }
        public List<TreeElementDto> Children { set; get; }
    }

    public class TreeElementDto
    {
        /*Index tree*/
        public int Id { set; get; }
        public int Id_Db { set; get; }
        public int ParentId { set; get; }
        public int Type { set; get; }
        public string Text { set; get; }
        public bool Opened { set; get; }
        public List<TreeElementDto> Children { set; get; }
    }
}
