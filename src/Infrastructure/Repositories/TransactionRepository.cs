using Domain;
using Marten;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDocumentSession session;

        public TransactionRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public async Task AddAsync(IEnumerable<Transaction> transactions)
        {
            session.Store(transactions);

            await session.SaveChangesAsync();
        }
    }
}
