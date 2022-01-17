using MediatR;

namespace Domain
{
    // Taken from https://github.com/kgrzybek/modular-monolith-with-ddd/blob/master/src/BuildingBlocks/Domain/IDomainEvent.cs
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}
