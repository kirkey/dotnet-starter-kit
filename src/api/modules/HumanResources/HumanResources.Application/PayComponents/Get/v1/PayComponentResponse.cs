namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;

public sealed record PayComponentResponse(
    DefaultIdType Id,
    string Code,
    string ComponentName,
    string ComponentType,
    string CalculationMethod,
    string? CalculationFormula,
    decimal? Rate,
    decimal? FixedAmount,
    decimal? MinValue,
    decimal? MaxValue,
    string GlAccountCode,
    bool IsActive,
    bool IsCalculated,
    bool IsMandatory,
    bool IsSubjectToTax,
    bool IsTaxExempt,
    string? LaborLawReference,
    string? Description,
    int DisplayOrder,
    bool AffectsGrossPay,
    bool AffectsNetPay);

