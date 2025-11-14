namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Command to update tax bracket details.
/// </summary>
public sealed record UpdateTaxBracketCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] decimal? MinIncome = null,
    [property: DefaultValue(null)] decimal? MaxIncome = null,
    [property: DefaultValue(null)] decimal? Rate = null,
    [property: DefaultValue(null)] string? FilingStatus = null,
    [property: DefaultValue(null)] string? Description = null
) : IRequest<UpdateTaxBracketResponse>;

/// <summary>
/// Response for tax bracket update.
/// </summary>
public sealed record UpdateTaxBracketResponse(
    DefaultIdType Id,
    string TaxType,
    int Year,
    decimal MinIncome,
    decimal MaxIncome,
    decimal Rate);

