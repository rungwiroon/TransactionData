using Application.Commands.ImportTransactions;
using Domain.TransactionDomain.AggregateModel;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static LanguageExt.Prelude;

namespace Application.Tests
{
    public class ImportTransactionCommandHandlerTests
    {
        private TransactionItemDTO transactionItem = new()
        {
            TransactionID = "Invoice0000001",
            Amount = 1_000.00m,
            CurrencyCode = "USD",
            TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
            Status = "Approved",
        };

        public class MockedTransactionRepository : ITransactionRepository
        {
            public int SaveCalled { get; private set; } = 0;
            public List<Transaction> Transactions { get; private set;} = new List<Transaction>();

            public void SaveChanges()
            {
                SaveCalled++;
            }

            public Task SaveChangesAsync(CancellationToken token = default)
            {
                SaveCalled++;

                return Task.CompletedTask;
            }

            public void Store(IEnumerable<Transaction> transactions)
            {
                Transactions.AddRange(transactions);
            }
        }

        [Fact]
        public async Task WhenHandleCsvFileCommand_ShouldStoreDataInRepository()
        {
            var mockedRepository = new MockedTransactionRepository();
            var handler = new ImportTransactionsHandler(mockedRepository);
            var creationResult = ImportCsvFileCommand.Create(new TransactionItemDTO[]
            {
                transactionItem
            });

            var handleResult = await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: errors => Left(errors));

            Assert.Single(mockedRepository.Transactions);
            Assert.Equal(1, mockedRepository.SaveCalled);
            Assert.True(handleResult.IsRight);

            var rowsSaved = handleResult.Match(
                Right: rowCount => rowCount,
                Left: _ => 0);

            Assert.Equal(1, rowsSaved);
        }

        [Fact]
        public async Task WhenHandleCsvFileCommandWithInvalidCurrency_ShouldReturnDomainException()
        {
            var mockedRepository = new MockedTransactionRepository();
            var handler = new ImportTransactionsHandler(mockedRepository);
            
            transactionItem.CurrencyCode = "AAA";
            var creationResult = ImportCsvFileCommand.Create(new TransactionItemDTO[]
            {
                transactionItem
            });

            var handleResult = await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: errors => Left(errors));

            Assert.Empty(mockedRepository.Transactions);
            Assert.Equal(0, mockedRepository.SaveCalled);
            Assert.True(handleResult.IsLeft);

            var errors = handleResult.Match(
                Right: _ => Enumerable.Empty<string>(),
                Left: errors => errors);

            Assert.Single(errors);
        }

        [Fact]
        public async Task WhenHandleXmlFileCommand_ShouldStoreDataInRepository()
        {
            var mockedRepository = new MockedTransactionRepository();
            var handler = new ImportTransactionsHandler(mockedRepository);
            var creationResult = ImportXmlFileCommand.Create(new TransactionItemDTO[]
            {
                transactionItem
            });

            await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: errors => Left(errors));

            Assert.Single(mockedRepository.Transactions);
            Assert.Equal(1, mockedRepository.SaveCalled);
        }

        [Fact]
        public async Task WhenHandleXmlFileCommandWithInvalidCurrency_ShouldReturnDomainException()
        {
            var mockedRepository = new MockedTransactionRepository();
            var handler = new ImportTransactionsHandler(mockedRepository);

            transactionItem.CurrencyCode = "AAA";
            var creationResult = ImportCsvFileCommand.Create(new TransactionItemDTO[]
            {
                transactionItem
            });

            var handleResult = await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: errors => Left(errors));

            Assert.Empty(mockedRepository.Transactions);
            Assert.Equal(0, mockedRepository.SaveCalled);
            Assert.True(handleResult.IsLeft);

            var errors = handleResult.Match(
                Right: _ => Enumerable.Empty<string>(),
                Left: errors => errors);

            Assert.Single(errors);
        }
    }
}
