namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Approve.v1;

/// <summary>
/// Response after approving an insurance claim.
/// </summary>
public sealed record ApproveInsuranceClaimResponse(DefaultIdType Id, decimal ApprovedAmount, string Status);
