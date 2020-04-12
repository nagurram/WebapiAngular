using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class MenuItems
    {
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public string Link { get; set; }
        public int? Parentmenuid { get; set; }
        public int? Sortorder { get; set; }

        public virtual Menus Menu { get; set; }
    }
}
