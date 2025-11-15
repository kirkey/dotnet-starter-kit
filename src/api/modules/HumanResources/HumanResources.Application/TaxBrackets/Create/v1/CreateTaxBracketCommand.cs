namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;

/// <summary>
/// Command to create a new tax bracket for Philippines Income Tax (TRAIN Law).
/// Philippines uses progressive tax system with brackets based on annual income.
/// </summary>
public sealed record CreateTaxBracketCommand(
    [property: DefaultValue("IncomeTax")] string TaxType,
    [property: DefaultValue(2025)] int Year,
    [property: DefaultValue(0)] decimal MinIncome,
    [property: DefaultValue(20833)] decimal MaxIncome,
    [property: DefaultValue(0)] decimal Rate,
    [property: DefaultValue(null)] string? FilingStatus = null,
    [property: DefaultValue(null)] string? Description = null
) : IRequest<CreateTaxBracketResponse>;
