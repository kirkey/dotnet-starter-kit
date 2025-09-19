namespace Accounting.Application.Budgets.Create;

/// <summary>
/// Response for creating a Budget; returns the created Budget identifier.
/// </summary>
/// <param name="Id">Identifier of the created Budget.</param>
public sealed record CreateBudgetResponse(DefaultIdType Id);

