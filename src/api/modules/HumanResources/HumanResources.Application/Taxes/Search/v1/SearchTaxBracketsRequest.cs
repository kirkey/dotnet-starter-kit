namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Request to search tax brackets.
/// </summary>
public sealed record SearchTaxBracketsRequest(
    [property: DefaultValue(null)] string? TaxType = null,
    [property: DefaultValue(null)] int? Year = null,
    [property: DefaultValue(null)] decimal? MinIncomeFrom = null,
    [property: DefaultValue(null)] decimal? MinIncomeTo = null,
    [property: DefaultValue(null)] string? FilingStatus = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(50)] int PageSize = 50
) : IRequest<PagedList<TaxBracketDto>>;

/// <summary>
/// DTO for tax bracket search results.
/// </summary>
public sealed record TaxBracketDto(
    DefaultIdType Id,
    string TaxType,
    int Year,
    decimal MinIncome,
    decimal MaxIncome,
    decimal Rate,
    string? FilingStatus);

