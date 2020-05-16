using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class PriorityMaster
    {
        public PriorityMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int PriorityId { get; set; }
        public string PriorityDescription { get; set; }
        public int? PriorityOrder { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
