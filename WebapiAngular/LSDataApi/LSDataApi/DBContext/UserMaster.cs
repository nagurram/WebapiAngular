using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
