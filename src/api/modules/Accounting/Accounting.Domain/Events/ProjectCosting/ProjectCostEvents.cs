namespace Accounting.Domain.Events.ProjectCosting;

/// <summary>
/// Domain event raised when a new project cost entry is created.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the created project cost entry.</param>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="Amount">The cost amount.</param>
/// <param name="Description">The description of the cost entry.</param>
/// <param name="Category">Optional cost category.</param>
public sealed record ProjectCostEntryCreated(
    DefaultIdType ProjectCostEntryId,
    DefaultIdType ProjectId,
    decimal Amount,
    string Description,
    string? Category) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is updated.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the updated project cost entry.</param>
public sealed record ProjectCostEntryUpdated(DefaultIdType ProjectCostEntryId) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is updated (identifier payload).
/// </summary>
public sealed record ProjectCostEntryUpdatedEvent(DefaultIdType ProjectCostEntryId) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is approved.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the approved project cost entry.</param>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="Amount">The approved cost amount.</param>
public sealed record ProjectCostEntryApproved(
    DefaultIdType ProjectCostEntryId,
    DefaultIdType ProjectId,
    decimal Amount) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is deleted.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the deleted project cost entry.</param>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="Amount">The cost amount that was deleted.</param>
public sealed record ProjectCostEntryDeleted(
    DefaultIdType ProjectCostEntryId,
    DefaultIdType ProjectId,
    decimal Amount) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is rejected during approval process.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the rejected project cost entry.</param>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="RejectionReason">The reason for rejection.</param>
/// <param name="RejectedBy">The person who rejected the cost entry.</param>
public sealed record ProjectCostEntryRejected(
    DefaultIdType ProjectCostEntryId,
    DefaultIdType ProjectId,
    string RejectionReason,
    string RejectedBy) : DomainEvent;

/// <summary>
/// Domain event raised when a project cost entry is allocated to a different cost center.
/// </summary>
/// <param name="ProjectCostEntryId">The unique identifier of the project cost entry.</param>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="PreviousCostCenter">The previous cost center.</param>
/// <param name="NewCostCenter">The new cost center.</param>
/// <param name="Amount">The amount being reallocated.</param>
public sealed record ProjectCostEntryReallocated(
    DefaultIdType ProjectCostEntryId,
    DefaultIdType ProjectId,
    string? PreviousCostCenter,
    string NewCostCenter,
    decimal Amount) : DomainEvent;
