using Accounting.Application.Projects.Queries;

namespace Accounting.Application.Projects.Costing.Get;

public sealed class GetProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository
) : IRequestHandler<GetProjectCostEntryQuery, ProjectCostResponse>
{
    public async Task<ProjectCostResponse> Handle(GetProjectCostEntryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        var entry = project.CostingEntries.FirstOrDefault(e => e.Id == request.EntryId)
                   ?? throw new JobCostingEntryNotFoundException(request.EntryId);

        return entry.Adapt<ProjectCostResponse>();
    }
}
