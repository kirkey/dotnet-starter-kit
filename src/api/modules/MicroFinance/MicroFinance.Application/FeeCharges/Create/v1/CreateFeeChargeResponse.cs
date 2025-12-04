namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;

/// <summary>
/// Response after creating fee charge.
/// </summary>
public sealed record CreateFeeChargeResponse(Guid Id, string Reference, decimal Amount);
