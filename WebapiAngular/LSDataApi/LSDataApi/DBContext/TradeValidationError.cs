namespace LSDataApi.DBContext
{
    public partial class TradeValidationError
    {
        public int TradeValidationErrorId { get; set; }
        public int? TradeId { get; set; }
        public string ErrorCode { get; set; }

        public virtual Trade Trade { get; set; }
    }
}