using Marten;

namespace Infrastructure
{
    public class CustomSessionFactory : ISessionFactory
    {
        private readonly IDocumentStore _store;

        public CustomSessionFactory(IDocumentStore store)
        {
            _store = store;
        }

        public IQuerySession QuerySession()
        {
            return _store.QuerySession();
        }

        public IDocumentSession OpenSession()
        {
            // Opting for the "lightweight" session
            // option with no identity map tracking
            return _store.LightweightSession();
        }
    }
}
