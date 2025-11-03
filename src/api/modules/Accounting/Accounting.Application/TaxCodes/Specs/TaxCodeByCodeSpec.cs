namespace Accounting.Application.TaxCodes.Specs;

/// <summary>
/// Specification to retrieve a tax code by its unique code.
/// Used for validation to prevent duplicate tax codes.
/// </summary>
public sealed class TaxCodeByCodeSpec : Specification<TaxCode>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaxCodeByCodeSpec"/> class.
    /// </summary>
    /// <param name="code">The unique tax code to search for.</param>
    public TaxCodeByCodeSpec(string code)
    {
        Query.Where(t => t.Code == code.Trim().ToUpperInvariant());
    }
}

