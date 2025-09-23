using Accounting.Application.Projects.Specifications;

namespace Accounting.Application.Projects.Get.v1;

/// <summary>
/// Handler for retrieving project details with comprehensive cost information.
/// </summary>
public sealed class GetProjectHandler(
    ILogger<GetProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<GetProjectQuery, GetProjectResponse>
{
    public async Task<GetProjectResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.GetBySpecAsync(
            new ProjectWithCostEntriesSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.Id);

        logger.LogInformation("project retrieved {ProjectId}", project.Id);

        return project.Adapt<GetProjectResponse>();
    }
}
