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
    
    public partial class TodoList
    {
        public int TodoId { get; set; }
        public string Titile { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> actionDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Userid { get; set; }
    }
}
