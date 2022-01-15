namespace Application.Queries
{
    public class TransactionViewModel
    {
        public string? TransactionID { get; set; }

        public decimal? Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public DateTime? TransactionDate { get; set; }

        public String? Status { get; set; }
    }
}
