using Domain;
using Marten;

namespace Infrastructure
{
    public static class DocumentMapper
    {
        public static void Config(StoreOptions opts)
        {
            opts.Schema.For<Transaction>()
                .Identity(t => t.TransactionID);
        }
    }
}
