using Accounting.Application.Projects.Queries;

namespace Accounting.Application.ProjectCosting.Create;

/// <summary>
/// Handles creation of a new job cost entry for a project aggregate and returns the created entry id.
/// </summary>
public sealed class CreateProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository
) : IRequestHandler<CreateProjectCostEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateProjectCostEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        // Delegate all business validation to the aggregate to keep rules DRY.
        var newEntry = project.AddCostEntry(
            request.Date,
            request.Description,
            request.Amount,
            request.AccountId,
            request.JournalEntryId,
            request.Category);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return newEntry.Id;
    }
}
