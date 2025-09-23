using Accounting.Application.Projects.Queries;

namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Handler for creating new projects with proper domain validation and event publishing.
/// </summary>
public sealed class CreateProjectHandler(
    ILogger<CreateProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
{
    public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate project name
        var existingProject = await repository.FirstOrDefaultAsync(
            new ProjectByNameSpec(request.Name), cancellationToken).ConfigureAwait(false);
        
        if (existingProject is not null)
        {
            throw new DuplicateProjectException(request.Name);
        }

        // Create the project using the domain factory method
        var project = Project.Create(
            request.Name,
            request.StartDate,
            request.BudgetedAmount,
            request.ClientName,
            request.ProjectManager,
            request.Department,
            request.Description,
            request.Notes);

        // Save to repository
        await repository.AddAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project created {ProjectId}", project.Id);

        return new CreateProjectResponse(project.Id);
    }
}
