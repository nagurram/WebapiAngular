using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int RoleId { get; set; }
        public string RoleDescription { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}