using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Projects.Create;

public sealed class CreateProjectHandler(
    [FromKeyedServices("accounting")] IRepository<Project> repository)
    : IRequestHandler<CreateProjectRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = Project.Create(
            request.Name,
            request.StartDate,
            request.BudgetedAmount,
            request.ClientName,
            request.ProjectManager,
            request.Department,
            request.Description,
            request.Notes);

        await repository.AddAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
