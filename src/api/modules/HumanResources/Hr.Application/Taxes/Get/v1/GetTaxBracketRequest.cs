namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Request to get tax bracket details.
/// </summary>
public sealed record GetTaxBracketRequest(DefaultIdType Id) : IRequest<TaxBracketResponse>;

/// <summary>
/// Response with tax bracket details.
/// </summary>
public sealed record TaxBracketResponse(
    DefaultIdType Id,
    string TaxType,
    int Year,
    decimal MinIncome,
    decimal MaxIncome,
    decimal Rate,
    string? FilingStatus,
    string? Description);

