using Application.Commands;
using System.Xml.Serialization;

namespace WebApp.Helpers
{
    public class XmlHelper
    {
		[XmlRoot(ElementName = "PaymentDetails")]
		public class PaymentDetails
		{
			[XmlElement(ElementName = "Amount")]
			public decimal? Amount { get; set; }
			
			[XmlElement(ElementName = "CurrencyCode")]
			public string? CurrencyCode { get; set; }
		}

		[XmlRoot(ElementName = "Transaction")]
		public class Transaction
		{
			[XmlElement(ElementName = "TransactionDate")]
			public DateTime? TransactionDate { get; set; }
			
			[XmlElement(ElementName = "PaymentDetails")]
			public PaymentDetails? PaymentDetails { get; set; }
			
			[XmlElement(ElementName = "Status")]
			public string? Status { get; set; }
			
			[XmlAttribute(AttributeName = "id")]
			public string? Id { get; set; }
		}

		[XmlRoot(ElementName = "Transactions")]
		public class Transactions
		{
			[XmlElement(ElementName = "Transaction")]
			public List<Transaction>? Transaction { get; set; }
		}

		public static IReadOnlyList<TransactionItem>? Read(Stream stream)
        {
			var reader = new XmlSerializer(typeof(Transactions));
			var transactions = (Transactions?)reader.Deserialize(stream);

			return transactions?.Transaction
				?.Select(tx => new TransactionItem()
                {
					TransactionID = tx.Id,
					Amount = tx?.PaymentDetails?.Amount,
					CurrencyCode = tx?.PaymentDetails?.CurrencyCode,
					TransactionDate = tx?.TransactionDate,
					Status = tx?.Status,
				})
				.ToList();
		}
	}
}
