namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;

public sealed record UpdatePayComponentCommand(
    DefaultIdType Id,
    [property: DefaultValue("Basic Pay")] string? ComponentName = null,
    [property: DefaultValue("Manual")] string? CalculationMethod = null,
    [property: DefaultValue("HourlyRate * Hours")] string? CalculationFormula = null,
    [property: DefaultValue(0.0)] decimal? Rate = null,
    [property: DefaultValue(0.0)] decimal? FixedAmount = null,
    [property: DefaultValue("6100")] string? GlAccountCode = null,
    [property: DefaultValue("Updated description")] string? Description = null,
    [property: DefaultValue(0)] int? DisplayOrder = null)
    : IRequest<UpdatePayComponentResponse>;

