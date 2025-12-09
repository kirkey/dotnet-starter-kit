namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Reject.v1;

/// <summary>
/// Response after rejecting a fee waiver.
/// </summary>
public sealed record RejectFeeWaiverResponse(DefaultIdType Id, string Status);
