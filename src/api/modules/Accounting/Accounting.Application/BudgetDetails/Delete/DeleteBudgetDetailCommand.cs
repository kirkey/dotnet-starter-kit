namespace Accounting.Application.BudgetDetails.Delete;

/// <summary>
/// Command to delete a budget detail by Id.
/// </summary>
public sealed record DeleteBudgetDetailCommand(DefaultIdType Id) : IRequest;

