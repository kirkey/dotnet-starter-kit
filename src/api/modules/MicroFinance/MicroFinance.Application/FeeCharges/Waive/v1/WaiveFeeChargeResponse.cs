namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Waive.v1;

/// <summary>
/// Response after waiving fee charge.
/// </summary>
public sealed record WaiveFeeChargeResponse(DefaultIdType FeeChargeId, string Status, string Message);
