using Accounting.Application.Projects.Queries;

namespace Accounting.Application.ProjectCosting.Update;

/// <summary>
/// Handles updating an existing job cost entry within a project aggregate.
/// Returns the updated entry identifier.
/// </summary>
public sealed class UpdateProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository
) : IRequestHandler<UpdateProjectCostEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateProjectCostEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new ProjectNotFoundException(request.ProjectId);

        // Delegate business rules to the aggregate (includes budget validation and state checks)
        project.UpdateCostEntry(request.EntryId, request.Date, request.Description, request.Amount, request.Category);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return request.EntryId;
    }
}
