namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Command to create a new tax bracket.
/// </summary>
public sealed record CreateTaxCommand(
    [property: DefaultValue("Federal")] string TaxType,
    [property: DefaultValue(2025)] int Year,
    [property: DefaultValue(0)] decimal MinIncome,
    [property: DefaultValue(100000)] decimal MaxIncome,
    [property: DefaultValue(0.10)] decimal Rate,
    [property: DefaultValue(null)] string? FilingStatus = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateTaxResponse>;

