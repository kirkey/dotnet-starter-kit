namespace Accounting.Application.Projects.Responses;

/// <summary>
/// Response model representing a project entity for API and CQRS queries.
/// Contains project information including timeline, budget, and performance metrics.
/// </summary>
public sealed record ProjectResponse(
    DefaultIdType Id,
    string Name,
    DateTime StartDate,
    DateTime? EndDate,
    decimal BudgetedAmount,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    decimal ActualCost,
    decimal ActualRevenue,
    string? Description,
    string? Notes,
    string? ImageUrl);
