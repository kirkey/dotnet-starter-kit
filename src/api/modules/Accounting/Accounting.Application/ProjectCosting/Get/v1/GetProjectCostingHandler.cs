namespace Accounting.Application.ProjectCosting.Get.v1;

/// <summary>
/// Handler for retrieving a single project costing entry.
/// </summary>
public sealed class GetProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IReadRepository<ProjectCostEntry> repository)
    : IRequestHandler<GetProjectCostingQuery, ProjectCostingResponse>
{
    /// <summary>
    /// Handles retrieving a project costing entry by ID.
    /// </summary>
    public async Task<ProjectCostingResponse> Handle(GetProjectCostingQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entry ?? throw new Domain.Exceptions.ProjectCostNotFoundException(request.Id);

        return new ProjectCostingResponse
        {
            Id = entry.Id,
            ProjectId = entry.ProjectId,
            EntryDate = entry.EntryDate,
            Amount = entry.Amount,
            Description = entry.Description,
            Category = entry.Category,
            AccountId = entry.AccountId,
            JournalEntryId = entry.JournalEntryId,
            CostCenter = entry.CostCenter,
            WorkOrderNumber = entry.WorkOrderNumber,
            IsBillable = entry.IsBillable,
            IsApproved = entry.IsApproved,
            Vendor = entry.Vendor,
            InvoiceNumber = entry.InvoiceNumber,
            CreatedOn = entry.CreatedOn,
            CreatedBy = entry.CreatedBy,
            LastModifiedOn = entry.LastModifiedOn,
            LastModifiedBy = entry.LastModifiedBy
        };
    }
}
