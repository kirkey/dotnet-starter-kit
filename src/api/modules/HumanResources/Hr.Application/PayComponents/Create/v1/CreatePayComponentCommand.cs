namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

/// <summary>
/// Command to create a new pay component for payroll calculation.
/// </summary>
public sealed record CreatePayComponentCommand(
    [property: DefaultValue("BASIC_PAY")] string Code,
    [property: DefaultValue("Basic Pay")] string ComponentName,
    [property: DefaultValue("Earnings")] string ComponentType,
    [property: DefaultValue("Manual")] string CalculationMethod,
    [property: DefaultValue("6100")] string? GlAccountCode = null,
    [property: DefaultValue("Basic monthly salary")] string? Description = null,
    [property: DefaultValue("HourlyRate * Hours")] string? CalculationFormula = null,
    [property: DefaultValue(0.0)] decimal? Rate = null,
    [property: DefaultValue(0.0)] decimal? FixedAmount = null,
    [property: DefaultValue(0.0)] decimal? MinValue = null,
    [property: DefaultValue(0.0)] decimal? MaxValue = null,
    [property: DefaultValue(true)] bool IsCalculated = false,
    [property: DefaultValue(false)] bool IsMandatory = false,
    [property: DefaultValue(true)] bool IsSubjectToTax = true,
    [property: DefaultValue(false)] bool IsTaxExempt = false,
    [property: DefaultValue("Labor Code Article 97")] string? LaborLawReference = null,
    [property: DefaultValue(0)] int DisplayOrder = 0,
    [property: DefaultValue(true)] bool AffectsGrossPay = false,
    [property: DefaultValue(true)] bool AffectsNetPay = true)
    : IRequest<CreatePayComponentResponse>;

