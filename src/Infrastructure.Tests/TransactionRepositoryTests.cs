using Domain;
using Marten;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests
{
    public class TransactionRepositoryTests
    {
        private readonly StoreOptions storeOptions;

        public TransactionRepositoryTests()
        {
            storeOptions = new StoreOptions();
            storeOptions.Connection("User ID=postgres;Password=example;Host=localhost;Port=5432;Database=transaction-data-dev;");
            DocumentMapper.Config(storeOptions);
        }

        [Fact]
        public async Task WhenStoreRecords_ShouldHaveRowsInDb()
        {
            var store = new DocumentStore(storeOptions);
            var repository = new TransactionRepository(store);
            var transactions1 = new Transaction[]
            {
                new Transaction(
                    "1",
                    new TransactionAmount(100.0m),
                    new CurrencyCode("THB"),
                    new DateTime(2022, 1, 15, 0, 0, 0),
                    TransactionStatus.A),
            };

            await repository.AddAsync(transactions1);
            var documents1 = (await repository.GetAllTransactionsAsync());

            Assert.Equal(1, documents1.Count);
            Assert.Equal(new TransactionAmount(100.0m), documents1[0].Amount);

            var transactions2 = new Transaction[]
            {
                new Transaction(
                    "1",
                    new TransactionAmount(200.0m),
                    new CurrencyCode("THB"),
                    new DateTime(2022, 1, 15, 0, 0, 0),
                    TransactionStatus.A),
            };

            await repository.AddAsync(transactions2);

            var documents2 = (await repository.GetAllTransactionsAsync());

            Assert.Equal(1, documents2.Count);
            Assert.Equal(new TransactionAmount(200.0m), documents2[0].Amount);
        }
    }
}