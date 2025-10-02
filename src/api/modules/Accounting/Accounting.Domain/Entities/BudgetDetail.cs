namespace Accounting.Domain.Entities;

/// <summary>
/// A single detail item within a budget representing the amount allocated to a specific account.
/// </summary>
/// <remarks>
/// Use cases:
/// - Allocate budget to an account (e.g., Salaries, Utilities).
/// - Track actuals posted against the allocation.
/// - Analyze variance at the account level.
///
/// Defaults:
/// - ActualAmount: 0.00 until updated.
/// - Description: null (optional note up to 500 chars).
/// </remarks>
public class BudgetDetail : AuditableEntity, IAggregateRoot
{
    private const int MaxBudgetLineDescriptionLength = 500;

    /// <summary>
    /// The parent Budget aggregate identifier.
    /// </summary>
    public DefaultIdType BudgetId { get; private set; }

    /// <summary>
    /// The Chart of Account identifier this detail applies to.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// The amount allocated for the account. Must be non-negative.
    /// </summary>
    public decimal BudgetedAmount { get; private set; }

    /// <summary>
    /// The actual amount recorded against the account (from GL postings).
    /// </summary>
    public decimal ActualAmount { get; private set; }

    // EF Core
    private BudgetDetail() { }

    private BudgetDetail(DefaultIdType budgetId, DefaultIdType accountId,
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        BudgetId = budgetId;
        AccountId = accountId;
        BudgetedAmount = budgetedAmount;
        ActualAmount = 0;

        var desc = description?.Trim();
        if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
            desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
        Description = desc;
    }

    /// <summary>
    /// Factory to create a new budget detail.
    /// </summary>
    public static BudgetDetail Create(DefaultIdType budgetId, DefaultIdType accountId,
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        return new BudgetDetail(budgetId, accountId, budgetedAmount, description);
    }

    /// <summary>
    /// Update budgeted amount and/or description. Amount must remain non-negative.
    /// </summary>
    public BudgetDetail Update(decimal? budgetedAmount, string? description)
    {
        if (budgetedAmount.HasValue && BudgetedAmount != budgetedAmount.Value)
        {
            if (budgetedAmount.Value < 0)
                throw new InvalidBudgetAmountException();
            BudgetedAmount = budgetedAmount.Value;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
                desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
            Description = desc;
        }

        return this;
    }

    /// <summary>
    /// Update the actual amount posted for this account.
    /// </summary>
    public BudgetDetail UpdateActual(decimal actualAmount)
    {
        ActualAmount = actualAmount;
        return this;
    }

    /// <summary>
    /// The variance for this line (Budgeted - Actual).
    /// </summary>
    public decimal GetVariance() => BudgetedAmount - ActualAmount;

    /// <summary>
    /// The percentage variance relative to the budgeted amount.
    /// </summary>
    public decimal GetVariancePercentage()
        => BudgetedAmount > 0 ? ((BudgetedAmount - ActualAmount) / BudgetedAmount) * 100 : 0;
}
