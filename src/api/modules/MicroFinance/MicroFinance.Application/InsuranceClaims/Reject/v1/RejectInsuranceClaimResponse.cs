namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Reject.v1;

/// <summary>
/// Response after rejecting an insurance claim.
/// </summary>
public sealed record RejectInsuranceClaimResponse(DefaultIdType Id, string Status);
