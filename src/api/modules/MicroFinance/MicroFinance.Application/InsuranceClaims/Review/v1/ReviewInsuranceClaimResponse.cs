namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Review.v1;

/// <summary>
/// Response after reviewing an insurance claim.
/// </summary>
public sealed record ReviewInsuranceClaimResponse(Guid Id, string Status);
