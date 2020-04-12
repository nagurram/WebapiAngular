using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class FinancialEventSource
    {
        public FinancialEventSource()
        {
            FinancialEventSourceFlatFile = new HashSet<FinancialEventSourceFlatFile>();
            FinancialEventSourceSwift = new HashSet<FinancialEventSourceSwift>();
        }

        public int FinancialEventSourceId { get; set; }
        public int? FinancialEventSourceTypeId { get; set; }
        public DateTime? CaptureDateTime { get; set; }

        public virtual ICollection<FinancialEventSourceFlatFile> FinancialEventSourceFlatFile { get; set; }
        public virtual ICollection<FinancialEventSourceSwift> FinancialEventSourceSwift { get; set; }
    }
}
