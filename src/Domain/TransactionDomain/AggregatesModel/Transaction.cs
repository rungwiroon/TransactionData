using Domain.Entities;
using Domain.TransactionDomain.Exceptions;

namespace Domain.TransactionDomain.AggregateModel
{
    public record struct CurrencyCode
    {
        public string Value { get; private set; }

        public static readonly string[] CurrencyList =
        {
            "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN",
            "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYN", "BZD",
            "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK",
            "DJF", "DKK", "DOP", "DZD",
            "EGP", "ERN", "ETB", "EUR",
            "FJD", "FKP",
            "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD",
            "HKD", "HNL", "HRK", "HTG", "HUF",
            "IDR", "ILS", "INR", "IQD", "IRR", "ISK",
            "JMD", "JOD", "JPY",
            "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT",
            "LAK", "LBP", "LKR", "LRD", "LSL", "LYD",
            "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRU", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZN",
            "NAD", "NGN", "NIO", "NOK", "NPR", "NZD",
            "OMR",
            "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG",
            "QAR",
            "RON", "RSD", "RUB", "RWF",
            "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "SSP", "STN", "SVC", "SYP", "SZL",
            "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS",
            "UAH", "UGX", "USD", "USN", "UYI", "UYU", "UYW", "UZS",
            "VED", "VES", "VND", "VUV",
            "WST",
            "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XSU", "XTS", "XUA", "XXX",
            "YER",
            "ZAR", "ZMW", "ZWL",
        };

        public CurrencyCode(string value)
        {
            if (!CurrencyList.Contains(value.ToUpper()))
                throw new TransactionDomainException($"Invalid currency code, value : {value}");

            Value = value.ToUpper();
        }
    }

    public record struct TransactionAmount
    {
        public decimal Value { get; private set; }

        public CurrencyCode Currency { get; private set; }

        public TransactionAmount(decimal value, CurrencyCode currency)
        {
            Value = value;
            Currency = currency;
        }
    }

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
            DateTime transactionDate,
            TransactionStatus status)
        {
            TransactionID = transactionID;
            Amount = amount;
            TransactionDate = transactionDate;
            Status = status;
        }

        public string TransactionID { get; private set; }

        public TransactionAmount Amount { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public TransactionStatus Status { get; private set; }
    }
}
