using Domain;
using LinqKit;
using Marten;

namespace Application.Queries
{
    public class TransactionQueries : ITransactionQueries
    {
        private readonly IQuerySession session;

        public TransactionQueries(IQuerySession session)
        {
            this.session = session;
        }

        private static readonly Func<Transaction, TransactionViewModel> mapper =
            (Transaction tx) =>
            new TransactionViewModel()
            {
                ID = tx.TransactionID,
                Payment = $"{tx.Amount.Value:0.00} {tx.CurrencyCode.Value}",
                Status = tx.Status.ToString(),
            };

        public async Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(
            CurrencyCode? currencyCode = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionStatus? status = null)
        {
            var predicate = PredicateBuilder.New<Transaction>();

            if (currencyCode != null)
                predicate = predicate.And(tx => tx.CurrencyCode.Value == currencyCode.Value.Value);

            if (startDate != null)
                predicate = predicate.And(tx => tx.TransactionDate >= startDate.Value);

            if (endDate != null)
                predicate = predicate.And(tx => tx.TransactionDate <= endDate.Value);

            if (status != null)
                predicate = predicate.And(tx => tx.Status == status.Value);
            
            var transactions = await session.Query<Transaction>()
                .Where(predicate)
                .ToAsyncEnumerable()
                .Select(mapper)
                .ToListAsync();

            return transactions;
        }
    }
}
