namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Cancel.v1;

/// <summary>
/// Response after cancelling a fee waiver.
/// </summary>
public sealed record CancelFeeWaiverResponse(DefaultIdType Id, string Status);
