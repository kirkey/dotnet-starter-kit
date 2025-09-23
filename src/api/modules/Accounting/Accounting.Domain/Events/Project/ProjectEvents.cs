namespace Accounting.Domain.Events.Project;

/// <summary>
/// Domain event raised when a new project is created.
/// </summary>
/// <param name="ProjectId">The unique identifier of the created project.</param>
/// <param name="Name">The name of the project.</param>
/// <param name="StartDate">The project start date.</param>
/// <param name="BudgetedAmount">The approved budget amount.</param>
/// <param name="Description">Optional project description.</param>
/// <param name="Notes">Optional project notes.</param>
public sealed record ProjectCreated(
    DefaultIdType ProjectId,
    string Name,
    DateTime StartDate,
    decimal BudgetedAmount,
    string? Description,
    string? Notes) : DomainEvent;

/// <summary>
/// Domain event raised when a project is updated.
/// Carries the project identifier to avoid passing aggregate instances in events.
/// </summary>
/// <param name="ProjectId">The unique identifier of the updated project.</param>
public sealed record ProjectUpdated(DefaultIdType ProjectId) : DomainEvent;

/// <summary>
/// Domain event raised when a project is completed.
/// </summary>
/// <param name="ProjectId">The unique identifier of the completed project.</param>
/// <param name="CompletionDate">The date when the project was completed.</param>
/// <param name="FinalActualCost">The final actual cost of the project.</param>
/// <param name="FinalActualRevenue">The final actual revenue of the project.</param>
/// <param name="BudgetVariance">The variance between budgeted and actual costs.</param>
public sealed record ProjectCompleted(
    DefaultIdType ProjectId,
    DateTime CompletionDate,
    decimal FinalActualCost,
    decimal FinalActualRevenue,
    decimal BudgetVariance) : DomainEvent;

/// <summary>
/// Domain event raised when a project is cancelled.
/// </summary>
/// <param name="ProjectId">The unique identifier of the cancelled project.</param>
/// <param name="CancellationDate">The date when the project was cancelled.</param>
/// <param name="Reason">The reason for cancellation.</param>
/// <param name="ActualCostAtCancellation">The actual cost at the time of cancellation.</param>
public sealed record ProjectCancelled(
    DefaultIdType ProjectId,
    DateTime CancellationDate,
    string Reason,
    decimal ActualCostAtCancellation) : DomainEvent;

/// <summary>
/// Domain event raised when a project budget is adjusted.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project.</param>
/// <param name="PreviousBudget">The previous budget amount.</param>
/// <param name="NewBudget">The new budget amount.</param>
/// <param name="AdjustmentReason">The reason for the budget adjustment.</param>
/// <param name="ApprovedBy">The person who approved the budget change.</param>
public sealed record ProjectBudgetAdjusted(
    DefaultIdType ProjectId,
    decimal PreviousBudget,
    decimal NewBudget,
    string AdjustmentReason,
    string ApprovedBy) : DomainEvent;

/// <summary>
/// Domain event raised when a cost entry is added to a project.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project.</param>
/// <param name="CostAmount">The amount of the cost entry.</param>
/// <param name="NewTotalActualCost">The new total actual cost after adding this entry.</param>
public sealed record ProjectCostAdded(
    DefaultIdType ProjectId,
    decimal CostAmount,
    decimal NewTotalActualCost) : DomainEvent;

/// <summary>
/// Domain event raised when revenue is recognized for a project.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project.</param>
/// <param name="RevenueAmount">The amount of revenue recognized.</param>
/// <param name="NewTotalActualRevenue">The new total actual revenue after adding this entry.</param>
public sealed record ProjectRevenueRecognized(
    DefaultIdType ProjectId,
    decimal RevenueAmount,
    decimal NewTotalActualRevenue) : DomainEvent;

/// <summary>
/// Domain event raised when a project status changes.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project.</param>
/// <param name="PreviousStatus">The previous status of the project.</param>
/// <param name="NewStatus">The new status of the project.</param>
/// <param name="StatusChangeReason">The reason for the status change.</param>
public sealed record ProjectStatusChanged(
    DefaultIdType ProjectId,
    string PreviousStatus,
    string NewStatus,
    string StatusChangeReason) : DomainEvent;
