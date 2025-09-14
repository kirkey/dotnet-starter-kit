namespace Store.Domain.Events;

public record WholesaleContractCreated : DomainEvent
{
    public WholesaleContract WholesaleContract { get; init; } = default!;
}

public record WholesaleContractActivated : DomainEvent
{
    public WholesaleContract WholesaleContract { get; init; } = default!;
}

public record WholesaleContractTerminated : DomainEvent
{
    public WholesaleContract WholesaleContract { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

public record WholesaleContractRenewed : DomainEvent
{
    public WholesaleContract WholesaleContract { get; init; } = default!;
}

public record WholesalePricingCreated : DomainEvent
{
    public WholesalePricing WholesalePricing { get; init; } = default!;
}

public record WholesalePricingUpdated : DomainEvent
{
    public WholesalePricing WholesalePricing { get; init; } = default!;
}

public record WholesalePricingDeactivated : DomainEvent
{
    public WholesalePricing WholesalePricing { get; init; } = default!;
}
