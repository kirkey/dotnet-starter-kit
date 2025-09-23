namespace Accounting.Application.Projects.Delete.v1;

/// <summary>
/// Handler for deleting projects with proper validation and business rule enforcement.
/// </summary>
public sealed class DeleteProjectHandler(
    ILogger<DeleteProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<DeleteProjectCommand, DeleteProjectResponse>
{
    public async Task<DeleteProjectResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.Id);

        // Business rule: Cannot delete projects with cost entries (must be cleaned up first)
        if (project.CostingEntries.Any())
        {
            logger.LogWarning("cannot delete project with cost entries {ProjectId}", project.Id);
            return new DeleteProjectResponse(
                ProjectId: request.Id,
                IsDeleted: false,
                Message: "Cannot delete project with existing cost entries. Please remove all cost entries first.");
        }

        // Business rule: Cannot delete completed projects (for audit trail)
        if (project.Status == "Completed")
        {
            logger.LogWarning("cannot delete completed project {ProjectId}", project.Id);
            return new DeleteProjectResponse(
                ProjectId: request.Id,
                IsDeleted: false,
                Message: "Cannot delete completed projects. Completed projects must be retained for audit purposes.");
        }

        await repository.DeleteAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project deleted {ProjectId}", project.Id);

        return new DeleteProjectResponse(
            ProjectId: request.Id,
            IsDeleted: true,
            Message: "Project deleted successfully.");
    }
}
