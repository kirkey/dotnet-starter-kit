using Accounting.Application.Projects.Queries;

namespace Accounting.Application.ProjectCosting.Get;

/// <summary>
/// Handles listing all cost entries for a project.
/// </summary>
public sealed class GetProjectCostEntriesHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository
) : IRequestHandler<GetProjectCostEntriesQuery, ICollection<ProjectCostResponse>>
{
    public async Task<ICollection<ProjectCostResponse>> Handle(GetProjectCostEntriesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        var responses = project.CostingEntries
            .Where(e => e.Amount > 0)
            .OrderByDescending(e => e.EntryDate)
            .Select(e => new ProjectCostResponse(
                e.Id,
                project.Id,
                e.EntryDate,
                e.Description,
                e.Amount,
                e.AccountId,
                e.JournalEntryId,
                e.Category,
                e.CostCenter,
                e.WorkOrderNumber,
                e.IsBillable,
                e.IsApproved,
                e.Vendor,
                e.InvoiceNumber))
            .ToList();

        return responses;
    }
}
