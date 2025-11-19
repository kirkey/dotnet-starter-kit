namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

using Entities;

/// <summary>
/// Event raised when a performance review is created.
/// </summary>
public record PerformanceReviewCreated : DomainEvent
{
    public PerformanceReview Review { get; init; } = default!;
}

/// <summary>
/// Event raised when a performance review is submitted.
/// </summary>
public record PerformanceReviewSubmitted : DomainEvent
{
    public PerformanceReview Review { get; init; } = default!;
}

/// <summary>
/// Event raised when a performance review is completed.
/// </summary>
public record PerformanceReviewCompleted : DomainEvent
{
    public PerformanceReview Review { get; init; } = default!;
}

/// <summary>
/// Event raised when a performance review is acknowledged by employee.
/// </summary>
public record PerformanceReviewAcknowledged : DomainEvent
{
    public PerformanceReview Review { get; init; } = default!;
}

