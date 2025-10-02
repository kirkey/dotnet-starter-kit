using Accounting.Application.Projects.Costing.Responses;

namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Handles retrieval of a single project costing entry by ID.
/// </summary>
public sealed class GetProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IReadRepository<ProjectCostEntry> repository)
    : IRequestHandler<GetProjectCostingQuery, ProjectCostingResponse?>
{
    /// <summary>
    /// Gets the project costing entry.
    /// </summary>
    public async Task<ProjectCostingResponse?> Handle(GetProjectCostingQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry is null)
            return null;

        return new ProjectCostingResponse(
            entry.Id,
            entry.ProjectId,
            entry.EntryDate,
            entry.Amount,
            entry.Description,
            entry.AccountId,
            entry.Category,
            entry.JournalEntryId,
            entry.CostCenter,
            entry.WorkOrderNumber,
            entry.IsBillable,
            entry.IsApproved,
            entry.Vendor,
            entry.InvoiceNumber);
    }
}
