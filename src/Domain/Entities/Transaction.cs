using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public record struct TransactionID
    {
        public TransactionID(string value)
        {
            if (value.Length < 8 && value.Length > 50)
                throw new ArgumentException("ID length must between 8 and 50");
            Value = value;
        }

        public string Value { get; }
    }

    public record struct CurrencyCode(string Value);

    public record struct TransactionDate(DateTime Value);

    public enum TransactionStatus
    {
        A,
        R,
        D,
    }

    public class Transaction : Entity, IAggregateRoot
    {
        public Transaction(
            TransactionID transactionID,
            decimal amount,
            CurrencyCode currencyCode,
            TransactionDate transactionDate,
            TransactionStatus status)
        {
            TransactionID = transactionID;
            Amount = amount;
            CurrencyCode = currencyCode;
            TransactionDate = transactionDate;
            Status = status;
        }

        public TransactionID TransactionID { get; private set; }

        public decimal Amount { get; private set; }

        public CurrencyCode CurrencyCode { get; private set; }

        public TransactionDate TransactionDate { get; private set; }

        public TransactionStatus Status { get; private set; }
    }
}
