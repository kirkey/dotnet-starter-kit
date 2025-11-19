namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Query to search tax master configurations.
/// Supports filtering and pagination.
/// </summary>
public sealed class SearchTaxesRequest : PaginationFilter, IRequest<PagedList<TaxDto>>
{
    /// <summary>
    /// Optional tax code to filter by.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Optional tax type to filter by (SalesTax, VAT, GST, etc.).
    /// </summary>
    public string? TaxType { get; set; }

    /// <summary>
    /// Optional jurisdiction to filter by.
    /// </summary>
    public string? Jurisdiction { get; set; }

    /// <summary>
    /// Optional flag to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Optional flag to filter by compound status.
    /// </summary>
    public bool? IsCompound { get; set; }
}

/// <summary>
/// DTO for tax search results.
/// </summary>
public sealed record TaxDto(
    DefaultIdType Id,
    string Code,
    string Name,
    string TaxType,
    decimal Rate,
    bool IsCompound,
    string? Jurisdiction,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    bool IsActive);
