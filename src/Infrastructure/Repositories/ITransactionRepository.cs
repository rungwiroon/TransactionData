using Domain;

namespace Infrastructure.Repositories
{
    public interface ITransactionRepository : IUnitOfWork
    {
        void Store(IEnumerable<Transaction> transactions);
    }
}
