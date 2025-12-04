namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;

public sealed record InsuranceProductResponse(
    Guid Id,
    string Code,
    string Name,
    string InsuranceType,
    string? Provider,
    decimal MinCoverage,
    decimal MaxCoverage,
    string PremiumCalculation,
    decimal PremiumRate,
    int? MinAge,
    int? MaxAge,
    int WaitingPeriodDays,
    bool PremiumUpfront,
    bool MandatoryWithLoan,
    string Status,
    string? Description,
    string? CoveredEvents,
    string? Exclusions,
    string? TermsConditions,
    string? Notes);
