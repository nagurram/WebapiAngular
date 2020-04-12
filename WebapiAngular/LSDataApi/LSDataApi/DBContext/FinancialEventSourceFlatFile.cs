using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class FinancialEventSourceFlatFile
    {
        public int FinancialEventSourceFlatFileId { get; set; }
        public int? FinancialEventSourceId { get; set; }
        public int? ClientProfileId { get; set; }
        public int? FlatFileProcessingStatusId { get; set; }
        public string FileName { get; set; }
        public string ReceiveDir { get; set; }
        public DateTime? ReceiveDateTime { get; set; }
        public string ProcessingNotes { get; set; }
        public int? LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }

        public virtual FinancialEventSource FinancialEventSource { get; set; }
    }
}
