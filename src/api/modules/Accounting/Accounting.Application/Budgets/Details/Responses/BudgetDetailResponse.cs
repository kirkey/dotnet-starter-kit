namespace Accounting.Application.Budgets.Details.Responses;

/// <summary>
/// Response model representing a budget detail item.
/// </summary>
public sealed class BudgetDetailResponse(
    DefaultIdType id,
    DefaultIdType budgetId,
    DefaultIdType accountId,
    decimal budgetedAmount,
    decimal actualAmount,
    string? description)
{
    public DefaultIdType Id { get; init; } = id;
    public DefaultIdType BudgetId { get; init; } = budgetId;
    public DefaultIdType AccountId { get; init; } = accountId;
    public decimal BudgetedAmount { get; init; } = budgetedAmount;
    public decimal ActualAmount { get; init; } = actualAmount;
    public string? Description { get; init; } = description;
}

