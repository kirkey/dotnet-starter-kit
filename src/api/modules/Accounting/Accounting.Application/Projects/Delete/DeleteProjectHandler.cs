using Accounting.Domain;
using Accounting.Application.Projects.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Projects.Delete;

public sealed class DeleteProjectHandler(
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<DeleteProjectRequest>
{
    public async Task Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null) throw new ProjectNotFoundException(request.Id);

        await repository.DeleteAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
