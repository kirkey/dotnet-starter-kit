using Accounting.Application.Budgets.Details.Responses;

namespace Accounting.Application.Budgets.Details.Specs;

/// <summary>
/// Specification to retrieve budget details by budget ID projected to <see cref="BudgetDetailResponse"/>.
/// Performs database-level projection for optimal performance in read-only operations.
/// </summary>
public sealed class GetBudgetDetailsSpec : Specification<BudgetDetail, BudgetDetailResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetBudgetDetailsSpec"/> class.
    /// </summary>
    /// <param name="budgetId">The budget identifier to filter budget details.</param>
    public GetBudgetDetailsSpec(DefaultIdType budgetId)
    {
        Query.Where(x => x.BudgetId == budgetId);

        Query.Select(d => new BudgetDetailResponse(
            d.Id,
            d.BudgetId,
            d.AccountId,
            d.BudgetedAmount,
            d.ActualAmount,
            d.Description
        ));
    }
}

