//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ang4api.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exception
    {
        public int ExceptionId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> ExceptionDateTime { get; set; }
        public string InnerExceptionmessage { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
