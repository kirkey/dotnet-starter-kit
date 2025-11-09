namespace Accounting.Application.AccountingPeriods.Responses;

/// <summary>
/// Response returned after a period close/reopen transition.
/// </summary>
public sealed record AccountingPeriodTransitionResponse(DefaultIdType Id, string Name, bool IsClosed);

