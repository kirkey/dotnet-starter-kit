using Accounting.Application.CreditMemos.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.CreditMemos.Search;

/// <summary>
/// Specification for searching credit memos with filtering and pagination.
/// </summary>
public class SearchCreditMemosSpec : EntitiesByPaginationFilterSpec<CreditMemo, CreditMemoResponse>
{
    public SearchCreditMemosSpec(SearchCreditMemosQuery request)
        : base(request)
    {
        Query
            .OrderByDescending(cm => cm.MemoDate, !request.HasOrderBy())
            .ThenByDescending(cm => cm.CreatedOn);

        // Filter by memo number
        if (!string.IsNullOrWhiteSpace(request.MemoNumber))
        {
            Query.Where(cm => cm.MemoNumber.Contains(request.MemoNumber));
        }

        // Filter by reference type
        if (!string.IsNullOrWhiteSpace(request.ReferenceType))
        {
            Query.Where(cm => cm.ReferenceType == request.ReferenceType);
        }

        // Filter by reference ID
        if (request.ReferenceId.HasValue)
        {
            Query.Where(cm => cm.ReferenceId == request.ReferenceId.Value);
        }

        // Filter by status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(cm => cm.Status == request.Status);
        }

        // Filter by approval status
        if (!string.IsNullOrWhiteSpace(request.ApprovalStatus))
        {
            Query.Where(cm => cm.ApprovalStatus == request.ApprovalStatus);
        }

        // Filter by amount range
        if (request.AmountFrom.HasValue)
        {
            Query.Where(cm => cm.Amount >= request.AmountFrom.Value);
        }

        if (request.AmountTo.HasValue)
        {
            Query.Where(cm => cm.Amount <= request.AmountTo.Value);
        }

        // Filter by memo date range
        if (request.MemoDateFrom.HasValue)
        {
            Query.Where(cm => cm.MemoDate >= request.MemoDateFrom.Value);
        }

        if (request.MemoDateTo.HasValue)
        {
            Query.Where(cm => cm.MemoDate <= request.MemoDateTo.Value);
        }

        // Filter by unapplied amount
        if (request.HasUnappliedAmount == true)
        {
            Query.Where(cm => (cm.Amount - cm.AppliedAmount - cm.RefundedAmount) > 0);
        }
    }
}
