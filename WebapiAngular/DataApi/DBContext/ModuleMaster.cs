//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataApi.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class ModuleMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleMaster()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Nullable<int> statusOrder { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual ModuleMaster ModuleMaster1 { get; set; }
        public virtual ModuleMaster ModuleMaster2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
