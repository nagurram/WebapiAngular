using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Trade
    {
        public Trade()
        {
            TradeValidationError = new HashSet<TradeValidationError>();
        }

        public int TradeId { get; set; }
        public int? TradeFileId { get; set; }
        public int? TradeTypeId { get; set; }
        public string ClientTradeRef { get; set; }
        public string SecurityCode { get; set; }
        public string Units { get; set; }
        public decimal? Price { get; set; }
        public int? Brokerage { get; set; }
        public decimal? BrokerageGst { get; set; }
        public decimal? NetSettlementValue { get; set; }
        public decimal? GrossSettlementValue { get; set; }

        public virtual ICollection<TradeValidationError> TradeValidationError { get; set; }
    }
}