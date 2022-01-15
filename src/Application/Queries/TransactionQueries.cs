using Domain;
using Marten;
using System.Linq.Expressions;

namespace Application.Queries
{
    public class TransactionQueries : ITransactionQueries
    {
        private readonly IQuerySession session;

        public TransactionQueries(IQuerySession session)
        {
            this.session = session;
        }

        private static readonly Expression<Func<Transaction, TransactionViewModel>> mapper =
            (Transaction tx) =>
            new TransactionViewModel()
            {
                TransactionID = tx.TransactionID,
                TransactionDate = tx.TransactionDate,
                Amount = tx.Amount.Value,
            };

        public async Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(
            CurrencyCode currencyCode)
        {
            var transactions = await session.Query<Transaction>()
                .Where(tx => tx.CurrencyCode.Value == currencyCode.Value)
                .Select(mapper)
                .ToListAsync();
            return transactions;
        }

        public async Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(
            DateTime startDate, DateTime endDate)
        {
            var transactions = await session.Query<Transaction>()
                .Where(tx => tx.TransactionDate >= startDate && tx.TransactionDate <= endDate)
                .Select(mapper)
                .ToListAsync();
            return transactions;
        }

        public async Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(
            TransactionStatus status)
        {
            var transactions = await session.Query<Transaction>()
                .Where(tx => tx.Status == status)
                .Select(mapper)
                .ToListAsync();
            return transactions;
        }
    }
}
