namespace Accounting.Application.Projects.Dtos;

public record ProjectDto(
    DefaultIdType Id,
    string Name,
    DateTime StartDate,
    DateTime? EndDate,
    decimal BudgetedAmount,
    string Status,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    decimal ActualCost,
    decimal ActualRevenue,
    string? Description,
    string? Notes);

public record JobCostingEntryDto(
    DefaultIdType Id,
    DefaultIdType ProjectId,
    DateTime EntryDate,
    decimal Amount,
    string CostType,
    string? Reference,
    string? Description);
