using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class ModuleMaster
    {
        public ModuleMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? StatusOrder { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
