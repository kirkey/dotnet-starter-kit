using Accounting.Application.Projects.Queries;

namespace Accounting.Application.ProjectCosting.Delete;

/// <summary>
/// Handles deletion of a project cost entry from the project aggregate.
/// </summary>
public sealed class DeleteProjectCostEntryHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository
) : IRequestHandler<DeleteProjectCostEntryCommand>
{
    public async Task Handle(DeleteProjectCostEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(
            new ProjectWithCostEntriesByIdSpec(request.ProjectId), cancellationToken);
        if (project is null)
            throw new Accounting.Application.Projects.Exceptions.ProjectNotFoundException(request.ProjectId);

        project.RemoveCostEntry(request.EntryId);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
