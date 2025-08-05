using Accounting.Domain;
using Accounting.Application.Projects.Dtos;
using Accounting.Application.Projects.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Projects.Get;

public sealed class GetProjectHandler(
    [FromKeyedServices("accounting")] IReadRepository<Project> repository)
    : IRequestHandler<GetProjectRequest, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null) throw new ProjectNotFoundException(request.Id);

        return new ProjectDto(
            project.Id,
            project.Name!,
            project.StartDate,
            project.EndDate,
            project.BudgetedAmount,
            project.Status,
            project.ClientName,
            project.ProjectManager,
            project.Department,
            project.ActualCost,
            project.ActualRevenue,
            project.Description,
            project.Notes);
    }
}
