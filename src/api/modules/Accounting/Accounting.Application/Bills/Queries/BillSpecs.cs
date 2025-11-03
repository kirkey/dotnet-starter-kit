namespace Accounting.Application.Bills.Queries;

/// <summary>
/// Specification for searching bills with filters.
/// </summary>
public sealed class SearchBillsSpec : Specification<Bill>
{
    public SearchBillsSpec(
        string? keyword = null,
        string? billNumber = null,
        DefaultIdType? vendorId = null,
        string? status = null,
        string? approvalStatus = null,
        DateTime? billDateFrom = null,
        DateTime? billDateTo = null,
        DateTime? dueDateFrom = null,
        DateTime? dueDateTo = null,
        bool? isPosted = null,
        bool? isPaid = null,
        DefaultIdType? periodId = null)
    {
        Query
            .Where(b => !string.IsNullOrWhiteSpace(keyword)
                ? b.BillNumber.Contains(keyword) || (b.Description != null && b.Description.Contains(keyword))
                : true)
            .Where(b => !string.IsNullOrWhiteSpace(billNumber) && b.BillNumber.Contains(billNumber), !string.IsNullOrWhiteSpace(billNumber))
            .Where(b => vendorId.HasValue && b.VendorId == vendorId.Value, vendorId.HasValue)
            .Where(b => !string.IsNullOrWhiteSpace(status) && b.Status == status, !string.IsNullOrWhiteSpace(status))
            .Where(b => !string.IsNullOrWhiteSpace(approvalStatus) && b.ApprovalStatus == approvalStatus, !string.IsNullOrWhiteSpace(approvalStatus))
            .Where(b => billDateFrom.HasValue && b.BillDate >= billDateFrom.Value, billDateFrom.HasValue)
            .Where(b => billDateTo.HasValue && b.BillDate <= billDateTo.Value, billDateTo.HasValue)
            .Where(b => dueDateFrom.HasValue && b.DueDate >= dueDateFrom.Value, dueDateFrom.HasValue)
            .Where(b => dueDateTo.HasValue && b.DueDate <= dueDateTo.Value, dueDateTo.HasValue)
            .Where(b => isPosted.HasValue && b.IsPosted == isPosted.Value, isPosted.HasValue)
            .Where(b => isPaid.HasValue && b.IsPaid == isPaid.Value, isPaid.HasValue)
            .Where(b => periodId.HasValue && b.PeriodId == periodId.Value, periodId.HasValue);
    }
}

/// <summary>
/// Specification for getting a bill by ID with line items.
/// </summary>
public sealed class GetBillByIdSpec : Specification<Bill>, ISingleResultSpecification<Bill>
{
    public GetBillByIdSpec(DefaultIdType billId)
    {
        Query
            .Where(b => b.Id == billId);
    }
}
