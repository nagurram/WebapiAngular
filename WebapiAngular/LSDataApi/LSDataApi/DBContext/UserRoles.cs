namespace LSDataApi.DBContext
{
    public partial class UserRoles
    {
        public int Mappingid { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual RoleMaster Role { get; set; }
        public virtual UserMaster User { get; set; }
    }
}