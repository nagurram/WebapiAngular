using System;

namespace LSDataApi.DBContext
{
    public partial class Exception
    {
        public int ExceptionId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int? UserId { get; set; }
        public DateTime? ExceptionDateTime { get; set; }
        public string InnerExceptionmessage { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}