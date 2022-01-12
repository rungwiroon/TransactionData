using System;
using System.IO;
using System.Text;
using Xunit;

namespace WebApp.Tests
{
    public class CsvHelperTest
    {
        [Fact]
        public void WhenReadEmptyContent_ShouldReturnEmptyList()
        {
            var content = string.Empty;

            var bytes = Encoding.ASCII.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            var result = Helpers.CsvHelper.Read(stream);

            Assert.Empty(result);
        }

        [Fact]
        public void WhenReadValidContent_ShouldReturnNonEmptyList()
        {
            var content = @"""Invoice0000001"",""1,000.00"", ""USD"", ""20/02/2019 12:33:16"", ""Approved""
""Invoice0000002"",""300.00"",""USD"",""21/02/2019 02:04:59"", ""Failed""";

            var bytes = Encoding.ASCII.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            var result = Helpers.CsvHelper.Read(stream);

            Assert.Equal(2, result.Count);

            var firstRecord = result[0];
            Assert.Equal("Invoice0000001", firstRecord.TransactionID);
            Assert.Equal(1000.00m, firstRecord.Amount);
            Assert.Equal("USD", firstRecord.CurrencyCode);
            Assert.Equal(new DateTime(2019, 2, 20, 12, 33, 16), firstRecord.TransactionDate);
            Assert.Equal("Approved", firstRecord.Status);
        }
    }
}