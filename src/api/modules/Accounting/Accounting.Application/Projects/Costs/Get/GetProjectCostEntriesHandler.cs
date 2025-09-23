using Accounting.Application.Projects.Queries;
using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Costs.Get;

/// <summary>
/// Handles listing all cost entries for a project.
/// </summary>
public sealed class GetProjectCostEntriesHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository
) : IRequestHandler<GetProjectCostEntriesQuery, ICollection<ProjectCostEntryResponse>>
{
    public async Task<ICollection<ProjectCostEntryResponse>> Handle(GetProjectCostEntriesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new Accounting.Application.Projects.Exceptions.ProjectNotFoundException(request.ProjectId);

        var responses = project.CostingEntries
            .Where(e => e.Amount > 0)
            .OrderByDescending(e => e.Date)
            .Select(e => new ProjectCostEntryResponse(
                e.Id,
                project.Id,
                e.Date,
                e.Description,
                e.Amount,
                e.AccountId,
                e.JournalEntryId,
                e.Category))
            .ToList();

        return responses;
    }
}
