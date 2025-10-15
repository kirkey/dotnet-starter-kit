using Accounting.Application.Checks.Get.v1;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Specs;

/// <summary>
/// Specification to retrieve a check by ID projected to <see cref="CheckGetResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetCheckSpec : Specification<Check, CheckGetResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCheckSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the check to retrieve.</param>
    public GetCheckSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

