namespace Accounting.Application.Checks.Search.v1;

/// <summary>
/// Query to search checks with filtering and pagination.
/// </summary>
public class CheckSearchQuery : PaginationFilter, IRequest<PagedList<CheckSearchResponse>>
{
    public string? CheckNumber { get; set; }
    public string? BankAccountCode { get; set; }
    public string? Status { get; set; }
    public string? PayeeName { get; set; }
    public DefaultIdType? VendorId { get; set; }
    public DefaultIdType? PayeeId { get; set; }
    public DateTime? IssuedDateFrom { get; set; }
    public DateTime? IssuedDateTo { get; set; }
    public DateTime? ClearedDateFrom { get; set; }
    public DateTime? ClearedDateTo { get; set; }
    public bool? IsPrinted { get; set; }
    public bool? IsStopPayment { get; set; }
    public decimal? AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
}

