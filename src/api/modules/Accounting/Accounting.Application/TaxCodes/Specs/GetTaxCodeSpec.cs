using Accounting.Application.TaxCodes.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.TaxCodes.Specs;

/// <summary>
/// Specification to retrieve a tax code by ID projected to <see cref="TaxCodeResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetTaxCodeSpec : Specification<TaxCode, TaxCodeResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetTaxCodeSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the tax code to retrieve.</param>
    public GetTaxCodeSpec(DefaultIdType id)
    {
        Query.Where(t => t.Id == id);
    }
}

