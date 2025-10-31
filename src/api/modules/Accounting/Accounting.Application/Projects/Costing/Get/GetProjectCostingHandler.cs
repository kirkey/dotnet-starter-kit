using Accounting.Application.Projects.Costing.Responses;
using Accounting.Application.Projects.Costing.Specs;

namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Handles retrieval of a single project costing entry by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IReadRepository<ProjectCostEntry> repository)
    : IRequestHandler<GetProjectCostingQuery, ProjectCostingResponse?>
{
    /// <summary>
    /// Gets the project costing entry using specification projection.
    /// </summary>
    /// <param name="request">The query containing the project costing entry ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The project costing response, or null if not found.</returns>
    public async Task<ProjectCostingResponse?> Handle(GetProjectCostingQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetProjectCostingSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
    }
}
