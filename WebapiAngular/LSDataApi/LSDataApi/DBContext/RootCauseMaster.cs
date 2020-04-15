using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class RootCauseMaster
    {
        public RootCauseMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int RootCauseId { get; set; }
        public string Description { get; set; }
        public bool? Isdelete { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}