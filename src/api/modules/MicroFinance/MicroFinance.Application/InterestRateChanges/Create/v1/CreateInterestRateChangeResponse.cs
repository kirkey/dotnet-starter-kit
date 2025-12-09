namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Create.v1;

/// <summary>
/// Response after creating an interest rate change.
/// </summary>
public sealed record CreateInterestRateChangeResponse(
    DefaultIdType Id,
    string Reference,
    string ChangeType,
    decimal PreviousRate,
    decimal NewRate,
    string Status);
