using Accounting.Application.Projects.Costing.Responses;

namespace Accounting.Application.Projects.Costing.Specs;

/// <summary>
/// Specification to retrieve a project costing entry by ID projected to <see cref="ProjectCostingResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetProjectCostingSpec : Specification<ProjectCostEntry, ProjectCostingResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectCostingSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the project costing entry to retrieve.</param>
    public GetProjectCostingSpec(DefaultIdType id)
    {
        Query.Where(e => e.Id == id);
    }
}

