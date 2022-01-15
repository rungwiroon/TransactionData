using Domain;

namespace Application.Queries
{
    public interface ITransactionQueries
    {
        Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(CurrencyCode currencyCode);
        Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(TransactionStatus status);
    }
}