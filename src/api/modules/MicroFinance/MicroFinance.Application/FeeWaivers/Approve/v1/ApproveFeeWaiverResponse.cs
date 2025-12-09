namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Approve.v1;

/// <summary>
/// Response after approving a fee waiver.
/// </summary>
public sealed record ApproveFeeWaiverResponse(DefaultIdType Id, string Status, decimal WaivedAmount);
