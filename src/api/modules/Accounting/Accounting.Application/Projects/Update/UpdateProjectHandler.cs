using Accounting.Application.Projects.Exceptions;
using Accounting.Application.Projects.Queries;
using ProjectNotFoundException = Accounting.Application.Projects.Exceptions.ProjectNotFoundException;

namespace Accounting.Application.Projects.Update;

public sealed class UpdateProjectHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<UpdateProjectRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null) throw new ProjectNotFoundException(request.Id);

        // Check for duplicate project name (excluding current project)
        if (!string.IsNullOrEmpty(request.Name) && request.Name != project.Name)
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new ProjectByNameSpec(request.Name), cancellationToken);
            if (existingByName != null)
            {
                throw new ProjectNameAlreadyExistsException(request.Name);
            }
        }

        project.Update(request.Name, request.StartDate, request.EndDate, 
                      request.BudgetedAmount, request.Status, request.ClientName,
                      request.ProjectManager, request.Department, 
                      request.Description, request.Notes);

        await repository.UpdateAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
