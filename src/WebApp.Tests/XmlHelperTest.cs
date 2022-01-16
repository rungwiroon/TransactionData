using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Tests
{
    public class XmlHelperTest
    {
        [Fact]
        public void WhenReadEmptyContent_ShouldThrowException()
        {
            var content = @"";

            var bytes = Encoding.UTF8.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            var act = () => Helpers.XmlHelper.Read(stream);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void WhenReadEmptyContent_ShouldReturnEmptyList()
        {
            var content = @"
<Transactions>
</Transactions>";

            var bytes = Encoding.UTF8.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            var transactions = Helpers.XmlHelper.Read(stream);

            Assert.Empty(transactions);
        }

        [Fact]
        public void WhenValidContent_ShouldReturnNonEmptyList()
        {
            var content = @"
<Transactions>
    <Transaction id=""Inv00001"">
        <TransactionDate>2019-01-23T13:45:10</TransactionDate>
        <PaymentDetails>
            <Amount>200.00</Amount>
            <CurrencyCode>USD</CurrencyCode>
        </PaymentDetails>
        <Status>Done</Status>
    </Transaction>
    <Transaction id=""Inv00002"">
        <TransactionDate>2019-01-24T16:09:15</TransactionDate>
        <PaymentDetails>
            <Amount>10000.00</Amount>
            <CurrencyCode>EUR</CurrencyCode>
        </PaymentDetails>
        <Status>Rejected</Status>
    </Transaction>
</Transactions>";

            var bytes = Encoding.ASCII.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            var transactions = Helpers.XmlHelper.Read(stream);

            Assert.NotNull(transactions);
            Assert.Equal(2, transactions?.Count);

            var firstRecord = transactions[0];
            Assert.Equal("Inv00001", firstRecord.TransactionID);
            Assert.Equal(200.00m, firstRecord.Amount);
            Assert.Equal("USD", firstRecord.CurrencyCode);
            Assert.Equal(new DateTime(2019, 1, 23, 13, 45, 10), firstRecord.TransactionDate);
            Assert.Equal("Done", firstRecord.Status);
        }
    }
}
