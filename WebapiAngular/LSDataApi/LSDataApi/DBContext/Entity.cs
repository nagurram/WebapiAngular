using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Entity
    {
        public Entity()
        {
            EntityLinkChildEntity = new HashSet<EntityLink>();
            EntityLinkParentEntity = new HashSet<EntityLink>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ReferenceId { get; set; }

        public virtual ICollection<EntityLink> EntityLinkChildEntity { get; set; }
        public virtual ICollection<EntityLink> EntityLinkParentEntity { get; set; }
    }
}
