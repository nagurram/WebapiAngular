using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Resource
    {
        public Resource()
        {
            Tickets = new HashSet<Tickets>();
            UserToken = new HashSet<UserToken>();
        }

        public int ResourceId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string Roles { get; set; }
        public bool Isactive { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
        public virtual ICollection<UserToken> UserToken { get; set; }
    }
}
