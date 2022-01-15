using Domain;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TransactionRepository : IRepository
    {
        private readonly IDocumentStore store;

        public TransactionRepository(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync()
        {
            using var session = store.QuerySession();
            var transactions = await session
                .Query<Transaction>()
                .ToListAsync();
            return transactions;
        }

        public async Task AddAsync(IEnumerable<Transaction> transactions)
        {
            using var session = store.LightweightSession();
            session.Store(transactions);

            await session.SaveChangesAsync();
        }
    }
}
