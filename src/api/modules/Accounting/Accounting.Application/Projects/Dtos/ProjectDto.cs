using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.Projects.Dtos;

public class ProjectDto(
    DateTime startDate,
    DateTime? endDate,
    decimal budgetedAmount,
    string? clientName,
    string? projectManager,
    string? department,
    decimal actualCost,
    decimal actualRevenue) : BaseDto
{
    public DateTime StartDate { get; set; } = startDate;
    public DateTime? EndDate { get; set; } = endDate;
    public decimal BudgetedAmount { get; set; } = budgetedAmount;
    public string? ClientName { get; set; } = clientName;
    public string? ProjectManager { get; set; } = projectManager;
    public string? Department { get; set; } = department;
    public decimal ActualCost { get; set; } = actualCost;
    public decimal ActualRevenue { get; set; } = actualRevenue;
}

public class JobCostingEntryDto(
    DefaultIdType projectId,
    DateTime entryDate,
    decimal amount,
    string costType,
    string? reference) : BaseDto
{
    public DefaultIdType ProjectId { get; set; } = projectId;
    public DateTime EntryDate { get; set; } = entryDate;
    public decimal Amount { get; set; } = amount;
    public string CostType { get; set; } = costType;
    public string? Reference { get; set; } = reference;
}
