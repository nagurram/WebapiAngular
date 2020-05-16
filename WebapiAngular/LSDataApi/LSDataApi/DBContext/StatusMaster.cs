using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class StatusMaster
    {
        public StatusMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public int? StatusOrder { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
