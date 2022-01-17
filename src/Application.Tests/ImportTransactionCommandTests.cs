using Application.Commands.ImportTransactions;
using System;
using System.Linq;
using Xunit;

namespace Application.Tests
{
    public class ImportTransactionCommandTests
    {
        [Fact]
        public void WhenCreateCsvFileCommandWithValidData_ShouldReturnCommandObject()
        {
            var items = new TransactionItemDTO[]
            {
                new TransactionItemDTO()
                {
                    TransactionID = "Invoice0000001",
                    Amount = 1_000.00m,
                    CurrencyCode = "USD",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Approved",
                }
            };

            var createCommandResult = ImportCsvFileCommand.Create(items);

            Assert.True(createCommandResult.IsRight);

            var transactions = createCommandResult.Match(
                Right: cmd => cmd.Items,
                Left: _ => Enumerable.Empty<TransactionItemDTO>());

            Assert.Single(transactions);
        }

        [Fact]
        public void WhenCreateCsvFileCommandWithInvalidTransactionID_ShouldReturnErrorList()
        {
            var items = new TransactionItemDTO[]
            {
                new TransactionItemDTO()
                {
                    TransactionID = "",
                    Amount = 1_000.00m,
                    CurrencyCode = "A",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Invalid",
                }
            };

            var createCommandResult = ImportCsvFileCommand.Create(items);

            Assert.True(createCommandResult.IsLeft);

            var errors = createCommandResult.Match(
                Right: _ => null,
                Left: errors => errors);

            Assert.Equal(3, errors.Count());
        }

        [Fact]
        public void WhenCreateXmlFileCommandWithValidData_ShouldReturnCommandObject()
        {
            var items = new TransactionItemDTO[]
            {
                new TransactionItemDTO()
                {
                    TransactionID = "Inv00001",
                    Amount = 200.00m,
                    CurrencyCode = "USD",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Done",
                }
            };

            var createCommandResult = ImportXmlFileCommand.Create(items);

            Assert.True(createCommandResult.IsRight);

            var transactions = createCommandResult.Match(
                Right: cmd => cmd.Items,
                Left: _ => Enumerable.Empty<TransactionItemDTO>());

            Assert.Single(transactions);
        }

        [Fact]
        public void WhenCreateXmlFileCommandWithInvalidData_ShouldReturnErrorList()
        {
            var items = new TransactionItemDTO[]
            {
                new TransactionItemDTO()
                {
                    TransactionID = "",
                    Amount = 1_000.00m,
                    CurrencyCode = "B",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Bad",
                }
            };

            var createCommandResult = ImportXmlFileCommand.Create(items);

            var errors = createCommandResult.Match(
                Right: _ => null,
                Left: errors => errors);

            Assert.Equal(3, errors.Count());
        }
    }
}
