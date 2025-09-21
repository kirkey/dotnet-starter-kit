// Models for BudgetDetails component (moved out of code-behind so Razor can resolve them at compile time)
namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

/// <summary>
/// Client-side response model for displaying budget details in the UI.
/// Mirrors server response shape.
/// </summary>
public class BudgetDetailResponse
{
    /// <summary>
    /// Unique identifier for the budget detail.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Parent budget identifier.
    /// </summary>
    public DefaultIdType BudgetId { get; set; }

    /// <summary>
    /// Account identifier for this budget detail.
    /// </summary>
    public DefaultIdType AccountId { get; set; }

    /// <summary>
    /// Budgeted amount for the detail.
    /// </summary>
    public decimal BudgetedAmount { get; set; }

    /// <summary>
    /// Actual amount for the detail.
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }
}

public class BudgetDetailViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType BudgetId { get; set; }
    public DefaultIdType AccountId { get; set; }
    public decimal BudgetedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public string? Description { get; set; }
}
