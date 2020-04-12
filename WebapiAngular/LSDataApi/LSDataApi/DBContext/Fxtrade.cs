using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Fxtrade
    {
        public int FxtradeId { get; set; }
        public int? FinancialEventId { get; set; }
        public int? PortfolioId { get; set; }
        public int? BrokerId { get; set; }
        public DateTime? TradeDate { get; set; }
        public DateTime? SettlementDate { get; set; }
        public decimal? SpotForward { get; set; }
        public decimal? Rate { get; set; }
        public string HiportRef { get; set; }
        public int? TradeCurrencyId { get; set; }
        public int? CounterPartyCurrencyId { get; set; }
        public string TradeNominal { get; set; }
        public string CounterPartyNominal { get; set; }
        public int? Position { get; set; }
        public string SenderToReceieverInfo { get; set; }
        public int? TradeAllocationFinancialEventId { get; set; }
        public int? MatchStatusId { get; set; }
    }
}
