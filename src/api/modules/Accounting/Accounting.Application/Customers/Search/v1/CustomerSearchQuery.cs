namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Query to search customers with pagination support.
/// Includes filtering by customer number, name, type, status, and active state.
/// </summary>
public class CustomerSearchQuery : PaginationFilter, IRequest<PagedList<CustomerSearchResponse>>
{
    /// <summary>
    /// Filter by customer number (partial match).
    /// </summary>
    public string? CustomerNumber { get; set; }

    /// <summary>
    /// Filter by customer name (partial match).
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Filter by customer type (exact match).
    /// Values: "Individual", "Business", "Government", "NonProfit", "Wholesale", "Retail".
    /// </summary>
    public string? CustomerType { get; set; }

    /// <summary>
    /// Filter by status (exact match).
    /// Values: "Active", "Inactive", "CreditHold", "PastDue", "Collections", "Closed".
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by active state.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by credit hold state.
    /// </summary>
    public bool? IsOnCreditHold { get; set; }

    /// <summary>
    /// Filter by tax exempt status.
    /// </summary>
    public bool? TaxExempt { get; set; }
}

