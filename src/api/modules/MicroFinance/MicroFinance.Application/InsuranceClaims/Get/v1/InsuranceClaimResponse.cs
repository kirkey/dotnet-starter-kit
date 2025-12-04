namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;

/// <summary>
/// Response containing insurance claim details.
/// </summary>
public sealed record InsuranceClaimResponse(
    Guid Id,
    Guid InsurancePolicyId,
    string ClaimNumber,
    string ClaimType,
    decimal ClaimAmount,
    decimal? ApprovedAmount,
    DateOnly IncidentDate,
    DateOnly FiledDate,
    string Status,
    string? Description,
    string? RejectionReason,
    DateOnly? PaymentDate);
