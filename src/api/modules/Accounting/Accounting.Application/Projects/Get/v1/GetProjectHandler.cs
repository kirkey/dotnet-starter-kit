using Accounting.Application.Projects.Specifications;

namespace Accounting.Application.Projects.Get.v1;

/// <summary>
/// Handler for retrieving project details with comprehensive cost information.
/// Uses specification pattern with database-level projection for optimal performance.
/// </summary>
public sealed class GetProjectHandler(
    ILogger<GetProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository)
    : IRequestHandler<GetProjectQuery, GetProjectResponse>
{
    /// <summary>
    /// Handles the get project query request.
    /// </summary>
    /// <param name="request">The get project query containing the project ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The project response with comprehensive details and cost information.</returns>
    /// <exception cref="ProjectNotFoundException">Thrown when project is not found.</exception>
    public async Task<GetProjectResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetProjectSpec(request.Id);
        var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.Id);

        logger.LogInformation("Project retrieved {ProjectId}", request.Id);

        return response;
    }
}
