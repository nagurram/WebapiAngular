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
    
    public partial class Trade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trade()
        {
            this.TradeValidationErrors = new HashSet<TradeValidationError>();
        }
    
        public int TradeId { get; set; }
        public Nullable<int> TradeFileID { get; set; }
        public Nullable<int> TradeTypeId { get; set; }
        public string ClientTradeRef { get; set; }
        public string SecurityCode { get; set; }
        public string Units { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Brokerage { get; set; }
        public Nullable<decimal> BrokerageGST { get; set; }
        public Nullable<decimal> NetSettlementValue { get; set; }
        public Nullable<decimal> GrossSettlementValue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TradeValidationError> TradeValidationErrors { get; set; }
    }
}
