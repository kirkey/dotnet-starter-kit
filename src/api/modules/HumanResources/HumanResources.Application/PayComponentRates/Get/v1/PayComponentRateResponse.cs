namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;

public sealed record PayComponentRateResponse(
    DefaultIdType Id,
    DefaultIdType PayComponentId,
    decimal MinAmount,
    decimal MaxAmount,
    decimal? EmployeeRate,
    decimal? EmployerRate,
    decimal? AdditionalEmployerRate,
    decimal? EmployeeAmount,
    decimal? EmployerAmount,
    decimal? TaxRate,
    decimal? BaseAmount,
    decimal? ExcessRate,
    int Year,
    DateTime? EffectiveStartDate,
    DateTime? EffectiveEndDate,
    bool IsActive,
    string? Description);

