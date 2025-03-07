﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Domain.Events;

namespace FSH.Framework.Core.Domain;

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;
    [NotMapped]
    public Collection<DomainEvent> DomainEvents { get; } = [];

    protected void QueueDomainEvent(DomainEvent @event)
    {
        if (!DomainEvents.Contains(@event))
            DomainEvents.Add(@event);
    }
}

public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = DefaultIdType.NewGuid();
}
