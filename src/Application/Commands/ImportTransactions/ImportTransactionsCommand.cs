using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class TransactionItem
    {
        public string? TransactionID { get; set; }

        public decimal? Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public DateTime? TransactionDate { get; set; }

        public String? Status { get; set; }
    }

    public class ImportTransactionsCommand : ICommand
    {
        private IReadOnlyCollection<TransactionItem> _items;

        public ImportTransactionsCommand(IReadOnlyCollection<TransactionItem> items)
        {
            _items = items;
        }
    }
}
