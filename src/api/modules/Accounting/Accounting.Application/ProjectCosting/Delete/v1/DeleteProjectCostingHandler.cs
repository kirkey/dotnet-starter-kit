namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Handler for deleting project costing entries.
/// </summary>
public sealed class DeleteProjectCostingHandler(
    ILogger<DeleteProjectCostingHandler> logger,
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<DeleteProjectCostingCommand, DeleteProjectCostingResponse>
{
    /// <summary>
    /// Handles the deletion of a project costing entry.
    /// </summary>
    public async Task<DeleteProjectCostingResponse> Handle(DeleteProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entry ?? throw new Domain.Exceptions.ProjectCostNotFoundException(request.Id);

        entry.MarkForDeletion();
        await repository.DeleteAsync(entry, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Project costing entry with ID {EntryId} deleted successfully", entry.Id);

        return new DeleteProjectCostingResponse(entry.Id);
    }
}
