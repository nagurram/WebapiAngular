using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class ApplicationMaster
    {
        public ApplicationMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int? StatusOrder { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}