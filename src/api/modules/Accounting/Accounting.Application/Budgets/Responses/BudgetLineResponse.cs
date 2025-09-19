namespace Accounting.Application.Budgets.Responses;

/// <summary>
/// Response model representing a budget line item.
/// Contains detailed line information for each account within a budget.
/// </summary>
public class BudgetLineResponse(
    DefaultIdType id,
    DefaultIdType budgetId,
    DefaultIdType accountId,
    decimal budgetedAmount,
    decimal actualAmount,
    string? description)
{
    /// <summary>
    /// Unique identifier for the budget line.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
    
    /// <summary>
    /// Reference to the parent budget.
    /// </summary>
    public DefaultIdType BudgetId { get; set; } = budgetId;
    
    /// <summary>
    /// Account identifier for this budget line.
    /// </summary>
    public DefaultIdType AccountId { get; set; } = accountId;
    
    /// <summary>
    /// Originally budgeted amount for this account.
    /// </summary>
    public decimal BudgetedAmount { get; set; } = budgetedAmount;
    
    /// <summary>
    /// Actual amount spent/received for this account.
    /// </summary>
    public decimal ActualAmount { get; set; } = actualAmount;
    
    /// <summary>
    /// Optional description or notes for this budget line.
    /// </summary>
    public string? Description { get; set; } = description;
}
