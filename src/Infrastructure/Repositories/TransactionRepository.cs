using Domain.TransactionDomain.AggregateModel;
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

        public void SaveChanges()
        {
            session.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken token = default)
            => session.SaveChangesAsync(token);

        public void Store(IEnumerable<Transaction> transactions)
        {
            session.Store(transactions);
        }
    }
}
