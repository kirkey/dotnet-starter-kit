using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Specs;

/// <summary>
/// Specification to retrieve a depreciation method by ID projected to <see cref="DepreciationMethodResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetDepreciationMethodSpec : Specification<DepreciationMethod, DepreciationMethodResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDepreciationMethodSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the depreciation method to retrieve.</param>
    public GetDepreciationMethodSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}


