using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Get;

public sealed class GetProjectHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository,
    ICacheService cache)
    : IRequestHandler<GetProjectQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"project:{request.Id}",
            async () =>
            {
                var project = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (project == null) throw new ProjectNotFoundException(request.Id);
                return project.Adapt<ProjectDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
