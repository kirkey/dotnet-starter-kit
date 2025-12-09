namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Cancel.v1;

/// <summary>
/// Response after cancelling an interest rate change.
/// </summary>
public sealed record CancelInterestRateChangeResponse(DefaultIdType Id, string Status);
