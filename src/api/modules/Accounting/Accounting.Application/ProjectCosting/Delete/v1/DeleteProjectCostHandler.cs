namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Handler for deleting project cost entries with proper validation and business rule enforcement.
/// </summary>
public sealed class DeleteProjectCostHandler(
    ILogger<DeleteProjectCostHandler> logger,
    [FromKeyedServices("accounting:projectcosts")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<DeleteProjectCostCommand, DeleteProjectCostResponse>
{
    public async Task<DeleteProjectCostResponse> Handle(DeleteProjectCostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var projectCost = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectCostNotFoundException(request.Id);

        // Business rule: Cannot delete approved cost entries (for audit trail)
        if (projectCost.IsApproved)
        {
            logger.LogWarning("cannot delete approved project cost {ProjectCostId}", projectCost.Id);
            return new DeleteProjectCostResponse(
                ProjectCostId: request.Id,
                IsDeleted: false,
                Message: "Cannot delete approved cost entries. Approved entries must be retained for audit purposes.");
        }

        // Use domain method to mark for deletion which will trigger events
        projectCost.MarkForDeletion();

        await repository.DeleteAsync(projectCost, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project cost deleted {ProjectCostId}", projectCost.Id);

        return new DeleteProjectCostResponse(
            ProjectCostId: request.Id,
            IsDeleted: true,
            Message: "Project cost entry deleted successfully.");
    }
}
