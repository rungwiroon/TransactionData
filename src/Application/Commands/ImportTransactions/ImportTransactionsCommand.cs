using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class TransactionItem
    {
        public string TransactionID { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public String Status { get; private set; }
    }

    public class ImportTransactionsCommand : ICommand
    {
        private IEnumerable<TransactionItem> items;

        public ImportTransactionsCommand(IEnumerable<TransactionItem> items)
        {
            this.items = items;
        }
    }
}
