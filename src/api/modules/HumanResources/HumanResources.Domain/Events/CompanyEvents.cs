
namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when a new company is created.
/// </summary>
public record CompanyCreated : DomainEvent
{
    public Company Company { get; init; } = default!;
}

/// <summary>
/// Event raised when a company is updated.
/// </summary>
public record CompanyUpdated : DomainEvent
{
    public Company Company { get; init; } = default!;
}

/// <summary>
/// Event raised when a company is activated.
/// </summary>
public record CompanyActivated : DomainEvent
{
    public DefaultIdType CompanyId { get; init; }
}

/// <summary>
/// Event raised when a company is deactivated.
/// </summary>
public record CompanyDeactivated : DomainEvent
{
    public DefaultIdType CompanyId { get; init; }
}

