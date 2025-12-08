namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

/// <summary>
/// Response after submitting an insurance claim.
/// </summary>
public sealed record SubmitInsuranceClaimResponse(DefaultIdType Id, string ClaimNumber);
