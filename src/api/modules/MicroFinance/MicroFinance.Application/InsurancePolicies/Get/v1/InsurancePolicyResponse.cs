namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;

/// <summary>
/// Response containing insurance policy details.
/// </summary>
public sealed record InsurancePolicyResponse(
    Guid Id,
    Guid MemberId,
    Guid InsuranceProductId,
    string PolicyNumber,
    decimal PremiumAmount,
    decimal CoverageAmount,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    Guid? LoanId,
    string? BeneficiaryName,
    decimal TotalPremiumPaid,
    int ClaimsCount);
