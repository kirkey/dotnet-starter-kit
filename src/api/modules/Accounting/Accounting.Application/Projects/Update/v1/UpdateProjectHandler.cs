using Accounting.Application.Projects.Queries;

namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Handler for updating existing projects with proper domain validation and event publishing.
/// </summary>
public sealed class UpdateProjectHandler(
    ILogger<UpdateProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    public async Task<UpdateProjectResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get the existing project
        var project = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.Id);

        // Check for duplicate name if name is being changed
        if (!string.IsNullOrWhiteSpace(request.Name) && project.Name != request.Name)
        {
            var existingProject = await repository.SingleOrDefaultAsync(
                new ProjectByNameSpec(request.Name), cancellationToken).ConfigureAwait(false);
            
            if (existingProject is not null)
            {
                throw new DuplicateProjectException(request.Name);
            }
        }

        // Update the project using domain method
        project.Update(request.Name,
            request.StartDate,
            request.EndDate,
            request.BudgetedAmount,
            request.Status,
            request.ClientName,
            request.ProjectManager,
            request.Department,
            request.Description,
            request.Notes);

        // Save changes
        await repository.UpdateAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project updated {ProjectId}", project.Id);

        return new UpdateProjectResponse(project.Id);
    }
}
