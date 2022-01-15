using Domain.Entities;

namespace Domain
{
    public record struct TransactionAmount(decimal Value);

    public record struct CurrencyCode(string Value);

    public enum TransactionStatus
    {
        A,
        R,
        D,
    }

    public class Transaction : Entity, IAggregateRoot
    {
        public Transaction(
            string transactionID,
            TransactionAmount amount,
            CurrencyCode currencyCode,
            DateTime transactionDate,
            TransactionStatus status)
        {
            TransactionID = transactionID;
            Amount = amount;
            CurrencyCode = currencyCode;
            TransactionDate = transactionDate;
            Status = status;
        }

        public string TransactionID { get; private set; }

        public TransactionAmount Amount { get; private set; }

        public CurrencyCode CurrencyCode { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public TransactionStatus Status { get; private set; }
    }
}
