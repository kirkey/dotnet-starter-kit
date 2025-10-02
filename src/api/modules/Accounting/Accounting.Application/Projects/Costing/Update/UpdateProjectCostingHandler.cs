using Accounting.Application.Projects.Costing.Exceptions;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Update;

/// <summary>
/// Handles updating an existing project costing entry.
/// </summary>
/// <remarks>
/// Validates that:
/// - The project costing entry exists.
/// - The entry is not approved (cannot modify approved entries).
/// On success, updates the entry and persists changes.
/// </remarks>
public sealed class UpdateProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<UpdateProjectCostingCommand>
{
    /// <summary>
    /// Updates the project costing entry.
    /// </summary>
    public async Task Handle(UpdateProjectCostingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry is null)
            throw new ProjectCostEntryNotFoundException(request.Id);

        // Update the entry using the domain method
        entry.Update(
            request.EntryDate,
            request.Amount,
            request.Description,
            request.Category,
            request.CostCenter,
            request.WorkOrderNumber,
            request.IsBillable,
            request.Vendor,
            request.InvoiceNumber);

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
