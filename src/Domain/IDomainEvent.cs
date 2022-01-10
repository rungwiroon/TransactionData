using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    // Taken from https://github.com/kgrzybek/modular-monolith-with-ddd/blob/master/src/BuildingBlocks/Domain/IDomainEvent.cs
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}
