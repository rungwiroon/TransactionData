using Domain;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
