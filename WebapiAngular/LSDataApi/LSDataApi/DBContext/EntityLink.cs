namespace LSDataApi.DBContext
{
    public partial class EntityLink
    {
        public int LinkId { get; set; }
        public int? ParentEntityId { get; set; }
        public int? ChildEntityId { get; set; }

        public virtual Entity ChildEntity { get; set; }
        public virtual Entity ParentEntity { get; set; }
    }
}