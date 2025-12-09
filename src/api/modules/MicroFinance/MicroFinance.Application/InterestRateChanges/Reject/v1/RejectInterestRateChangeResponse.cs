namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Reject.v1;

/// <summary>
/// Response after rejecting an interest rate change.
/// </summary>
public sealed record RejectInterestRateChangeResponse(DefaultIdType Id, string Status);
