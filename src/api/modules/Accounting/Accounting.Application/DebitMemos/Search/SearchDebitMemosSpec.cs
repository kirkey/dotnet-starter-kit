using Accounting.Application.DebitMemos.Responses;

namespace Accounting.Application.DebitMemos.Search;

/// <summary>
/// Specification for searching debit memos with filtering and pagination.
/// </summary>
public class SearchDebitMemosSpec : EntitiesByPaginationFilterSpec<DebitMemo, DebitMemoResponse>
{
    public SearchDebitMemosSpec(SearchDebitMemosQuery request)
        : base(request)
    {
        Query
            .OrderByDescending(dm => dm.MemoDate, !request.HasOrderBy())
            .ThenByDescending(dm => dm.CreatedOn);

        // Filter by memo number
        if (!string.IsNullOrWhiteSpace(request.MemoNumber))
        {
            Query.Where(dm => dm.MemoNumber.Contains(request.MemoNumber));
        }

        // Filter by reference type
        if (!string.IsNullOrWhiteSpace(request.ReferenceType))
        {
            Query.Where(dm => dm.ReferenceType == request.ReferenceType);
        }

        // Filter by reference ID
        if (request.ReferenceId.HasValue)
        {
            Query.Where(dm => dm.ReferenceId == request.ReferenceId.Value);
        }

        // Filter by status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(dm => dm.Status == request.Status);
        }

        // Filter by approval status
        if (!string.IsNullOrWhiteSpace(request.ApprovalStatus))
        {
            Query.Where(dm => dm.ApprovalStatus == request.ApprovalStatus);
        }

        // Filter by amount range
        if (request.AmountFrom.HasValue)
        {
            Query.Where(dm => dm.Amount >= request.AmountFrom.Value);
        }

        if (request.AmountTo.HasValue)
        {
            Query.Where(dm => dm.Amount <= request.AmountTo.Value);
        }

        // Filter by memo date range
        if (request.MemoDateFrom.HasValue)
        {
            Query.Where(dm => dm.MemoDate >= request.MemoDateFrom.Value);
        }

        if (request.MemoDateTo.HasValue)
        {
            Query.Where(dm => dm.MemoDate <= request.MemoDateTo.Value);
        }

        // Filter by unapplied amount
        if (request.HasUnappliedAmount == true)
        {
            Query.Where(dm => dm.Amount > dm.AppliedAmount);
        }
    }
}
