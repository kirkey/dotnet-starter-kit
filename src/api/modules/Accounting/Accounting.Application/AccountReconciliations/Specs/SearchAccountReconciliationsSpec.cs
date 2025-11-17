using Accounting.Application.AccountReconciliations.Search.v1;

namespace Accounting.Application.AccountReconciliations.Specs;

/// <summary>
/// Specification for searching account reconciliations.
/// </summary>
public sealed class SearchAccountReconciliationsSpec : Specification<AccountReconciliation>
{
    public SearchAccountReconciliationsSpec(SearchAccountReconciliationsRequest request)
    {
        Query.Where(r => true);

        if (request.GeneralLedgerAccountId.HasValue && request.GeneralLedgerAccountId != DefaultIdType.Empty)
            Query.Where(r => r.GeneralLedgerAccountId == request.GeneralLedgerAccountId);

        if (request.AccountingPeriodId.HasValue && request.AccountingPeriodId != DefaultIdType.Empty)
            Query.Where(r => r.AccountingPeriodId == request.AccountingPeriodId);

        if (!string.IsNullOrEmpty(request.ReconciliationStatus))
            Query.Where(r => r.ReconciliationStatus == request.ReconciliationStatus);

        if (!string.IsNullOrEmpty(request.SubsidiaryLedgerSource))
            Query.Where(r => r.SubsidiaryLedgerSource.Contains(request.SubsidiaryLedgerSource));

        if (request.HasVariance.HasValue && request.HasVariance.Value)
            Query.Where(r => r.Variance > 0);

        Query.OrderByDescending(r => r.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

