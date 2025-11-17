using Accounting.Application.AccountReconciliations.Responses;

namespace Accounting.Application.AccountReconciliations.Search.v1;

/// <summary>
/// Request to search account reconciliations.
/// </summary>
public class SearchAccountReconciliationsRequest : PaginationFilter, IRequest<PagedList<AccountReconciliationResponse>>
{
    /// <summary>
    /// Filter by GL account ID.
    /// </summary>
    public DefaultIdType? GeneralLedgerAccountId { get; set; }

    /// <summary>
    /// Filter by accounting period.
    /// </summary>
    public DefaultIdType? AccountingPeriodId { get; set; }

    /// <summary>
    /// Filter by reconciliation status.
    /// </summary>
    public string? ReconciliationStatus { get; set; }

    /// <summary>
    /// Filter by subsidiary ledger source.
    /// </summary>
    public string? SubsidiaryLedgerSource { get; set; }

    /// <summary>
    /// Filter by variance (non-zero only).
    /// </summary>
    public bool? HasVariance { get; set; }
}

