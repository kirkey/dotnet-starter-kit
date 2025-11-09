using Accounting.Application.Invoices.Get.v1;

namespace Accounting.Application.Invoices.Search.v1;

/// <summary>
/// Request to search invoices with filtering and pagination.
/// </summary>
public class SearchInvoicesRequest : PaginationFilter, IRequest<PagedList<InvoiceResponse>>
{
    /// <summary>
    /// Filter by member identifier.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by invoice number (partial match).
    /// </summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Filter by invoice status (Draft, Sent, Paid, Overdue, Cancelled, Voided).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Start of invoice date range (inclusive).
    /// </summary>
    public DateTime? InvoiceDateFrom { get; set; }

    /// <summary>
    /// End of invoice date range (inclusive).
    /// </summary>
    public DateTime? InvoiceDateTo { get; set; }

    /// <summary>
    /// Start of due date range (inclusive).
    /// </summary>
    public DateTime? DueDateFrom { get; set; }

    /// <summary>
    /// End of due date range (inclusive).
    /// </summary>
    public DateTime? DueDateTo { get; set; }

    /// <summary>
    /// Filter by billing period (e.g., "2025-08").
    /// </summary>
    public string? BillingPeriod { get; set; }

    /// <summary>
    /// Filter by consumption record identifier.
    /// </summary>
    public DefaultIdType? ConsumptionId { get; set; }

    /// <summary>
    /// Filter by rate schedule.
    /// </summary>
    public string? RateSchedule { get; set; }

    /// <summary>
    /// Minimum total amount filter.
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Maximum total amount filter.
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// Filter for invoices with outstanding balance only.
    /// </summary>
    public bool? HasOutstandingBalance { get; set; }
}

