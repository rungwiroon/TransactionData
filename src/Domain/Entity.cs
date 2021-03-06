using System.Runtime.Serialization;

namespace Domain
{
    // Taken from https://github.com/kgrzybek/modular-monolith-with-ddd/blob/master/src/BuildingBlocks/Domain/Entity.cs
    public abstract class Entity
    {
        private List<IDomainEvent>? _domainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent">Domain event.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();

            this._domainEvents.Add(domainEvent);
        }
    }
}
