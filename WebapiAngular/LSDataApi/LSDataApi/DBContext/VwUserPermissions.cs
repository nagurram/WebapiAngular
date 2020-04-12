using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class VwUserPermissions
    {
        public string Displayname { get; set; }
        public int Menuitemid { get; set; }
        public string Link { get; set; }
        public int? Parentmenuid { get; set; }
        public int? Sortorder { get; set; }
        public int Userid { get; set; }
        public int Roleid { get; set; }
    }
}
