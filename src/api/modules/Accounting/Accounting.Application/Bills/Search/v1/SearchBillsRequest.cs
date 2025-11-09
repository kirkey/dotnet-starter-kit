using Accounting.Application.Bills.Get.v1;

namespace Accounting.Application.Bills.Search.v1;

/// <summary>
/// Request used to search bills with pagination and filters.
/// </summary>
public class SearchBillsRequest : PaginationFilter, IRequest<PagedList<BillResponse>>
{
    /// <summary>
    /// Filter by vendor identifier.
    /// </summary>
    public DefaultIdType? VendorId { get; set; }

    /// <summary>
    /// Filter by bill number (partial match).
    /// </summary>
    public string? BillNumber { get; set; }

    /// <summary>
    /// Filter by bill status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Start of bill date range (inclusive).
    /// </summary>
    public DateTime? BillDateFrom { get; set; }

    /// <summary>
    /// End of bill date range (inclusive).
    /// </summary>
    public DateTime? BillDateTo { get; set; }

    /// <summary>
    /// Start of due date range (inclusive).
    /// </summary>
    public DateTime? DueDateFrom { get; set; }

    /// <summary>
    /// End of due date range (inclusive).
    /// </summary>
    public DateTime? DueDateTo { get; set; }

    /// <summary>
    /// Filter by posted state. When null, do not filter by posted state.
    /// </summary>
    public bool? IsPosted { get; set; }

    /// <summary>
    /// Filter by paid state. When null, do not filter by paid state.
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// Filter by accounting period identifier.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Filter by approval status: Pending, Approved, or Rejected.
    /// </summary>
    public string? ApprovalStatus { get; set; }
}

