using Domain;

namespace Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(IEnumerable<Transaction> transactions);
    }
}
