namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

/// <summary>
/// Response after submitting an insurance claim.
/// </summary>
public sealed record SubmitInsuranceClaimResponse(Guid Id, string ClaimNumber);
