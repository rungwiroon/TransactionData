namespace Application.Commands
{
    public class TransactionItemDTO
    {
        public string? TransactionID { get; set; }

        public decimal? Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public DateTime? TransactionDate { get; set; }

        public String? Status { get; set; }
    }

    public class ImportTransactionsCommand : ICommand
    {
        private IEnumerable<TransactionItemDTO> _items;

        private ImportTransactionsCommand(IEnumerable<TransactionItemDTO> items)
        {
            _items = items;
        }

        public ImportTransactionsCommand CreateFromCsvData(IEnumerable<TransactionItemDTO> items)
        {
            var mappedItems = items.Select(item => new TransactionItemDTO()
            {
                TransactionID = item.TransactionID,
                Amount = item.Amount,
                CurrencyCode = item.CurrencyCode,
                TransactionDate = item.TransactionDate,
                Status = item.Status,
            });

            return new ImportTransactionsCommand(mappedItems);
        }

        public ImportTransactionsCommand CreateFromXmlData(IEnumerable<TransactionItemDTO> items)
        {
            var mappedItems = items.Select(item => new TransactionItemDTO()
            {
                TransactionID = item.TransactionID,
                Amount = item.Amount,
                CurrencyCode = item.CurrencyCode,
                TransactionDate = item.TransactionDate,
                Status = item.Status,
            });

            return new ImportTransactionsCommand(mappedItems);
        }
    }
}
