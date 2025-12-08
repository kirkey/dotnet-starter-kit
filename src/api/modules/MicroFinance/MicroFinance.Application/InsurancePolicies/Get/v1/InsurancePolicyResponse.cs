namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;

/// <summary>
/// Response containing insurance policy details.
/// </summary>
public sealed record InsurancePolicyResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType InsuranceProductId,
    string PolicyNumber,
    decimal PremiumAmount,
    decimal CoverageAmount,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    DefaultIdType? LoanId,
    string? BeneficiaryName,
    string? BeneficiaryRelation,
    string? BeneficiaryContact,
    DateOnly? NextPremiumDue,
    DateOnly? WaitingPeriodEnd,
    decimal TotalPremiumPaid);
