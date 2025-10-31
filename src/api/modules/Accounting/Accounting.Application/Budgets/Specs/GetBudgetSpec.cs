using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Specs;

/// <summary>
/// Specification to retrieve a budget by ID projected to <see cref="BudgetResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetBudgetSpec : Specification<Budget, BudgetResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetBudgetSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the budget to retrieve.</param>
    public GetBudgetSpec(DefaultIdType id)
    {
        Query.Where(b => b.Id == id);
    }
}

