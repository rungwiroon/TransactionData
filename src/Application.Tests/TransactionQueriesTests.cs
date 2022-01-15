using Application.Queries;
using Domain;
using Infrastructure;
using Marten;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class TransactionQueriesTests
    {
        private readonly StoreOptions storeOptions;

        public TransactionQueriesTests()
        {
            storeOptions = new StoreOptions();
            storeOptions.Connection("User ID=postgres;Password=example;Host=localhost;Port=5432;Database=transaction-data-dev;");
            DocumentMapper.Config(storeOptions);
        }

        [Fact]
        public async Task WhenQueryBySpecificCurrencyCode_ShouldGetOnlyCorrespondingTransaction()
        {
            var store = new DocumentStore(storeOptions);
            var session = store.LightweightSession();
            var queries = new TransactionQueries(session);

            var transactions = await queries.GetTransactionsAsync(
                new CurrencyCode("THB"));

            Assert.Equal(1, transactions.Count);
        }

        [Fact]
        public async Task WhenQueryByDateRange_ShouldGetOnlyCorrespondingTransaction()
        {
            var store = new DocumentStore(storeOptions);
            var session = store.LightweightSession();
            var queries = new TransactionQueries(session);

            var transactions = await queries.GetTransactionsAsync(
                new DateTime(2022, 1, 15, 0, 0, 0),
                new DateTime(2022, 1, 16, 0, 0, 0));

            Assert.Equal(1, transactions.Count);
        }

        [Fact]
        public async Task WhenQueryByStatus_ShouldGetOnlyCorrespondingTransaction()
        {
            var store = new DocumentStore(storeOptions);
            var session = store.LightweightSession();
            var queries = new TransactionQueries(session);

            var transactions = await queries.GetTransactionsAsync(
                TransactionStatus.A);

            Assert.Equal(1, transactions.Count);
        }
    }
}