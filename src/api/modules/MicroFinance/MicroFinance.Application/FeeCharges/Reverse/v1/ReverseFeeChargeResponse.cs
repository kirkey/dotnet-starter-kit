namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Reverse.v1;

/// <summary>
/// Response after reversing fee charge.
/// </summary>
public sealed record ReverseFeeChargeResponse(Guid FeeChargeId, string Status, string Message);
