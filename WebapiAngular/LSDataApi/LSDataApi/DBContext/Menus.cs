using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Menus
    {
        public Menus()
        {
            MenuItems = new HashSet<MenuItems>();
        }

        public int MenuId { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<MenuItems> MenuItems { get; set; }
    }
}
