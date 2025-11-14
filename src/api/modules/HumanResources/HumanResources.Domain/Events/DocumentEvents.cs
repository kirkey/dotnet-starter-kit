using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when document template is created.
/// </summary>
public record DocumentTemplateCreated : DomainEvent
{
    public DocumentTemplate Template { get; init; } = default!;
}

/// <summary>
/// Event raised when document template is updated.
/// </summary>
public record DocumentTemplateUpdated : DomainEvent
{
    public DocumentTemplate Template { get; init; } = default!;
}

/// <summary>
/// Event raised when document is generated.
/// </summary>
public record DocumentGenerated : DomainEvent
{
    public GeneratedDocument Document { get; init; } = default!;
}

/// <summary>
/// Event raised when document is finalized.
/// </summary>
public record DocumentFinalized : DomainEvent
{
    public GeneratedDocument Document { get; init; } = default!;
}

/// <summary>
/// Event raised when document is signed.
/// </summary>
public record DocumentSigned : DomainEvent
{
    public GeneratedDocument Document { get; init; } = default!;
}

/// <summary>
/// Event raised when document is archived.
/// </summary>
public record DocumentArchived : DomainEvent
{
    public GeneratedDocument Document { get; init; } = default!;
}

