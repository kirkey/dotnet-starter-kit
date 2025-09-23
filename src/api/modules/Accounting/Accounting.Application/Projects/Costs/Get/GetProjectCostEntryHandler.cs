using Accounting.Application.Projects.Queries;
using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Costs.Get;

public sealed class GetProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository
) : IRequestHandler<GetProjectCostEntryQuery, ProjectCostEntryResponse>
{
    public async Task<ProjectCostEntryResponse> Handle(GetProjectCostEntryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new Accounting.Application.Projects.Exceptions.ProjectNotFoundException(request.ProjectId);

        var entry = project.CostingEntries.FirstOrDefault(e => e.Id == request.EntryId)
                   ?? throw new JobCostingEntryNotFoundException(request.EntryId);

        var response = new ProjectCostEntryResponse(
            entry.Id,
            project.Id,
            entry.Date,
            entry.Description,
            entry.Amount,
            entry.AccountId,
            entry.JournalEntryId,
            entry.Category);

        return response;
    }
}
