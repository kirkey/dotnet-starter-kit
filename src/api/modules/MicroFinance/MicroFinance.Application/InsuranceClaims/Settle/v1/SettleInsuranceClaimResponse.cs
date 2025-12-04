namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Settle.v1;

/// <summary>
/// Response after settling an insurance claim.
/// </summary>
public sealed record SettleInsuranceClaimResponse(Guid Id, decimal SettledAmount, DateOnly SettlementDate, string Status);
