namespace Accounting.Application.BudgetDetails.Create;

/// <summary>
/// Command to create a new budget detail entry for a specific budget and account.
/// </summary>
/// <param name="BudgetId">The parent budget identifier.</param>
/// <param name="AccountId">The chart of account identifier this detail applies to.</param>
/// <param name="BudgetedAmount">The non-negative amount allocated to the account.</param>
/// <param name="Description">Optional description up to 500 characters.</param>
public sealed record CreateBudgetDetailCommand(
    DefaultIdType BudgetId,
    DefaultIdType AccountId,
    decimal BudgetedAmount,
    string? Description
) : IRequest<DefaultIdType>;

