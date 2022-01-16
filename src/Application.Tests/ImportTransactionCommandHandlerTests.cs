using Application.Commands.ImportTransactions;
using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class ImportTransactionCommandHandlerTests
    {
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
                new TransactionItemDTO()
                {
                    TransactionID = "Invoice0000001",
                    Amount = 1_000.00m,
                    CurrencyCode = "USD",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Approved",
                }
            });

            await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: _ => false);

            Assert.Single(mockedRepository.Transactions);
            Assert.Equal(1, mockedRepository.SaveCalled);
        }

        [Fact]
        public async Task WhenHandleXmlFileCommand_ShouldStoreDataInRepository()
        {
            var mockedRepository = new MockedTransactionRepository();
            var handler = new ImportTransactionsHandler(mockedRepository);
            var creationResult = ImportXmlFileCommand.Create(new TransactionItemDTO[]
            {
                new TransactionItemDTO()
                {
                    TransactionID = "Inv00001",
                    Amount = 200.00m,
                    CurrencyCode = "USD",
                    TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                    Status = "Done",
                }
            });

            await creationResult.MatchAsync(
                RightAsync: async message => await handler.Handle(message, default),
                Left: _ => false);

            Assert.Single(mockedRepository.Transactions);
            Assert.Equal(1, mockedRepository.SaveCalled);
        }
    }
}
