using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for System, Configuration, and Utility entities.
/// </summary>

// Document Events
public sealed record DocumentCreated(Document Document) : DomainEvent;
public sealed record DocumentUpdated(Document Document) : DomainEvent;
public sealed record DocumentVerified(Guid DocumentId, string EntityType, Guid EntityId) : DomainEvent;
public sealed record DocumentExpired(Guid DocumentId, string EntityType, Guid EntityId) : DomainEvent;

// MfiConfiguration Events
public sealed record MfiConfigurationCreated(MfiConfiguration Config) : DomainEvent;
public sealed record MfiConfigurationUpdated(Guid ConfigId, string Key, string NewValue) : DomainEvent;
