namespace Accounting.Application.BudgetDetails.Update;

/// <summary>
/// Command to update a budget detail by its Id.
/// </summary>
public sealed record UpdateBudgetDetailCommand(
    DefaultIdType Id,
    DefaultIdType AccountId,
    decimal? BudgetedAmount,
    string? Description
) : IRequest<DefaultIdType>;

