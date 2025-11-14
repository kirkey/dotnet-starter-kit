namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

/// <summary>
/// Response with benefit details.
/// </summary>
public sealed record BenefitResponse(
    DefaultIdType Id,
    string BenefitName,
    string BenefitType,
    decimal EmployeeContribution,
    decimal EmployerContribution,
    bool IsMandatory,
    bool IsActive,
    DateTime EffectiveStartDate,
    DateTime? EffectiveEndDate,
    string? CoverageType,
    string? ProviderName,
    decimal? CoverageAmount,
    int? WaitingPeriodDays,
    string? Description);

