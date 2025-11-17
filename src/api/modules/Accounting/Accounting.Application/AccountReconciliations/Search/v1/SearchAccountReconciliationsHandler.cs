using Accounting.Application.AccountReconciliations.Responses;
using Accounting.Application.AccountReconciliations.Specs;

namespace Accounting.Application.AccountReconciliations.Search.v1;

/// <summary>
/// Handler for searching account reconciliations.
/// </summary>
public sealed class SearchAccountReconciliationsHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IReadRepository<AccountReconciliation> repository,
    ILogger<SearchAccountReconciliationsHandler> logger)
    : IRequestHandler<SearchAccountReconciliationsRequest, PagedList<AccountReconciliationResponse>>
{
    public async Task<PagedList<AccountReconciliationResponse>> Handle(
        SearchAccountReconciliationsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAccountReconciliationsSpec(request);
        var count = await repository.CountAsync(spec, cancellationToken);
        var items = await repository.ListAsync(spec, cancellationToken);

        logger.LogInformation("Retrieved {Count} account reconciliations", items.Count);

        var mappedItems = items.Select(r => new AccountReconciliationResponse
        {
            Id = r.Id,
            GeneralLedgerAccountId = r.GeneralLedgerAccountId,
            AccountingPeriodId = r.AccountingPeriodId,
            GlBalance = r.GlBalance,
            SubsidiaryLedgerBalance = r.SubsidiaryLedgerBalance,
            Variance = r.Variance,
            ReconciliationStatus = r.ReconciliationStatus,
            ReconciliationDate = r.ReconciliationDate,
            VarianceExplanation = r.VarianceExplanation,
            SubsidiaryLedgerSource = r.SubsidiaryLedgerSource,
            LineItemCount = r.LineItemCount,
            AdjustingEntriesRecorded = r.AdjustingEntriesRecorded,
            Status = r.Status,
            CreatedOn = r.CreatedOn,
            ApprovedBy = r.ApprovedBy,
            ApproverName = r.ApproverName,
            ApprovedOn = r.ApprovedOn,
            Remarks = r.Remarks,
            NotesCount = r.ReconciliationNotes.Count
        }).ToList();

        return new PagedList<AccountReconciliationResponse>(mappedItems, request.PageNumber, request.PageSize, count);
    }
}

