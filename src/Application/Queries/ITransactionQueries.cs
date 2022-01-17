using Domain;

namespace Application.Queries
{
    public interface ITransactionQueries
    {
        Task<IReadOnlyList<TransactionViewModel>> GetTransactionsAsync(
            CurrencyCode? currencyCode = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionStatus? status = null);
    }
}