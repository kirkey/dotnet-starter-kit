using Accounting.Application.Projects.Costing.Exceptions;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Delete;

/// <summary>
/// Handles deletion of a project costing entry.
/// </summary>
/// <remarks>
/// Validates that:
/// - The project costing entry exists.
/// - The entry is not approved (cannot delete approved entries).
/// On success, deletes the entry and persists changes.
/// </remarks>
public sealed class DeleteProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<DeleteProjectCostingCommand>
{
    /// <summary>
    /// Deletes the project costing entry.
    /// </summary>
    public async Task Handle(DeleteProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry is null)
            throw new ProjectCostEntryNotFoundException(request.Id);

        await repository.DeleteAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
