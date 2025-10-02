using Accounting.Application.ProjectCosting.Get.v1;

namespace Accounting.Application.ProjectCosting.Search.v1;

/// <summary>
/// Handler for searching project costing entries with filters.
/// </summary>
public sealed class SearchProjectCostingHandler(
    [FromKeyedServices("accounting:projectcosting")] IReadRepository<ProjectCostEntry> repository)
    : IRequestHandler<SearchProjectCostingQuery, PagedList<ProjectCostingResponse>>
{
    /// <summary>
    /// Handles searching project costing entries.
    /// </summary>
    public async Task<PagedList<ProjectCostingResponse>> Handle(SearchProjectCostingQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProjectCostingSpec(request);
        var entries = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = entries.Select(entry => new ProjectCostingResponse
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
        }).ToList();

        return new PagedList<ProjectCostingResponse>(responses, totalCount, request.PageNumber, request.PageSize);
    }
}
