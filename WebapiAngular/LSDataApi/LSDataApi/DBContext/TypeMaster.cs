using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class TypeMaster
    {
        public TypeMaster()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int TypeId { get; set; }
        public string TypeDescription { get; set; }
        public int? TypeOrder { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
