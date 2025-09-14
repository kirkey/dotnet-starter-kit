using Accounting.Application.Projects.Dtos;
using ProjectNotFoundException = Accounting.Application.Projects.Exceptions.ProjectNotFoundException;

namespace Accounting.Application.Projects.Get;

public sealed class GetProjectHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository,
    ICacheService cache)
    : IRequestHandler<GetProjectRequest, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectRequest request, CancellationToken cancellationToken)
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
