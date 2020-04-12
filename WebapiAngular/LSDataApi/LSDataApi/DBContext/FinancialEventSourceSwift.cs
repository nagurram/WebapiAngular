using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class FinancialEventSourceSwift
    {
        public int FinancialEventSourceSwiftId { get; set; }
        public int? FinancialEventSourceId { get; set; }
        public int? SwiftMessageId { get; set; }
        public int? SwiftMessageTypeId { get; set; }
        public int? BatchId { get; set; }
        public int? SenderBic { get; set; }
        public string Message { get; set; }
        public string MessageXml { get; set; }
        public DateTime? ReceiveDateTime { get; set; }

        public virtual FinancialEventSource FinancialEventSource { get; set; }
    }
}
