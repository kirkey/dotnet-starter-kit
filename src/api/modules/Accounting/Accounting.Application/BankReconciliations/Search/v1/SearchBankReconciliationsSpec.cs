using Accounting.Application.BankReconciliations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Search.v1;

/// <summary>
/// Specification for searching bank reconciliations with various filters and pagination support.
/// </summary>
public class SearchBankReconciliationsSpec : EntitiesByPaginationFilterSpec<BankReconciliation, BankReconciliationResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchBankReconciliationsSpec"/> class.
    /// </summary>
    /// <param name="request">The search bank reconciliations command containing filter criteria and pagination parameters.</param>
    public SearchBankReconciliationsSpec(SearchBankReconciliationsCommand request)
        : base(request)
    {
        Query
            .Where(r => r.BankAccountId == request.BankAccountId!.Value, request.BankAccountId.HasValue)
            .Where(r => r.ReconciliationDate >= request.FromDate!.Value, request.FromDate.HasValue)
            .Where(r => r.ReconciliationDate <= request.ToDate!.Value, request.ToDate.HasValue)
            .Where(r => r.Status.ToString() == request.Status, !string.IsNullOrEmpty(request.Status))
            .Where(r => r.IsReconciled == request.IsReconciled!.Value, request.IsReconciled.HasValue)
            .OrderByDescending(r => r.ReconciliationDate, !request.HasOrderBy());
    }
}
