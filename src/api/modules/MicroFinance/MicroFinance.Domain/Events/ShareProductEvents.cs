using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a share product is created.</summary>
public sealed record ShareProductCreated : DomainEvent
{
    public ShareProduct? ShareProduct { get; init; }
}

/// <summary>Domain event raised when a share product is updated.</summary>
public sealed record ShareProductUpdated : DomainEvent
{
    public ShareProduct? ShareProduct { get; init; }
}

/// <summary>Domain event raised when share product price is updated.</summary>
public sealed record ShareProductPriceUpdated : DomainEvent
{
    public DefaultIdType ProductId { get; init; }
    public decimal NewPrice { get; init; }
}
