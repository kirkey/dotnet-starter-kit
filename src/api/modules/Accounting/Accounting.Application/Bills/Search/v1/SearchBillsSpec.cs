namespace Accounting.Application.Bills.Search.v1;

/// <summary>
/// Specification for searching bills with filters and pagination.
/// </summary>
public sealed class SearchBillsSpec : Specification<Bill>
{
    public SearchBillsSpec(SearchBillsCommand filter)
    {
        Query.Include(b => b.LineItems);

        if (filter.VendorId.HasValue)
        {
            Query.Where(b => b.VendorId == filter.VendorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.BillNumber))
        {
            Query.Where(b => b.BillNumber.Contains(filter.BillNumber));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            Query.Where(b => b.Status == filter.Status);
        }

        if (filter.BillDateFrom.HasValue)
        {
            Query.Where(b => b.BillDate >= filter.BillDateFrom.Value);
        }

        if (filter.BillDateTo.HasValue)
        {
            Query.Where(b => b.BillDate <= filter.BillDateTo.Value);
        }

        if (filter.DueDateFrom.HasValue)
        {
            Query.Where(b => b.DueDate >= filter.DueDateFrom.Value);
        }

        if (filter.DueDateTo.HasValue)
        {
            Query.Where(b => b.DueDate <= filter.DueDateTo.Value);
        }

        if (filter.IsPosted.HasValue)
        {
            Query.Where(b => b.IsPosted == filter.IsPosted.Value);
        }

        if (filter.IsPaid.HasValue)
        {
            Query.Where(b => b.IsPaid == filter.IsPaid.Value);
        }

        if (filter.PeriodId.HasValue)
        {
            Query.Where(b => b.PeriodId == filter.PeriodId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ApprovalStatus))
        {
            Query.Where(b => b.ApprovalStatus == filter.ApprovalStatus);
        }

        // Ordering
        Query.OrderByDescending(b => b.BillDate);

        // Pagination
        if (filter.PageNumber > 0 && filter.PageSize > 0)
        {
            Query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }
    }
}
