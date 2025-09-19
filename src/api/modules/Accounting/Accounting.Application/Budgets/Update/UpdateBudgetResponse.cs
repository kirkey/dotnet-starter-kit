namespace Accounting.Application.Budgets.Update;

/// <summary>
/// Response for budget updates; returns the updated Budget Id.
/// </summary>
/// <param name="Id">Identifier of the updated Budget.</param>
public sealed record UpdateBudgetResponse(DefaultIdType Id);

