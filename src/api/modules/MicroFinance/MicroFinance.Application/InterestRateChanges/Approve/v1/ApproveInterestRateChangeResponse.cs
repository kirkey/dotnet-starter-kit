namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Approve.v1;

/// <summary>
/// Response after approving an interest rate change.
/// </summary>
public sealed record ApproveInterestRateChangeResponse(DefaultIdType Id, string Status, decimal NewRate);
