using Marten;

namespace Infrastructure
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken token = default(CancellationToken));
    }
}
