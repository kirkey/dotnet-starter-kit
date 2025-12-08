namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;

public sealed record FeeDefinitionResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    string FeeType,
    string CalculationType,
    string AppliesTo,
    string ChargeFrequency,
    decimal Amount,
    decimal? MinAmount,
    decimal? MaxAmount,
    bool IsTaxable,
    decimal? TaxRate,
    bool IsActive
);
