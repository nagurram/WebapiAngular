namespace LSDataApi.DBContext
{
    public partial class MenuPermission
    {
        public int MenuItemId { get; set; }
        public int? RoleId { get; set; }

        public virtual MenuItems MenuItem { get; set; }
        public virtual RoleMaster Role { get; set; }
    }
}