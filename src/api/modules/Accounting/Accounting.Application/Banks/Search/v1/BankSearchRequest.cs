using System.ComponentModel;
using Accounting.Application.Banks.Get.v1;

namespace Accounting.Application.Banks.Search.v1;

/// <summary>
/// Request for searching banks with filtering and pagination.
/// Follows the CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public class BankSearchRequest : PaginationFilter, IRequest<PagedList<BankResponse>>
{
    /// <summary>
    /// Bank code to filter by (partial match).
    /// </summary>
    [DefaultValue("")]
    public string? BankCode { get; set; }

    /// <summary>
    /// Bank name to filter by (partial match).
    /// </summary>
    [DefaultValue("")]
    public string? Name { get; set; }

    /// <summary>
    /// Routing number to filter by (exact match).
    /// </summary>
    [DefaultValue("")]
    public string? RoutingNumber { get; set; }

    /// <summary>
    /// SWIFT code to filter by (partial match).
    /// </summary>
    [DefaultValue("")]
    public string? SwiftCode { get; set; }

    /// <summary>
    /// Filter by active status (true/false/null for all).
    /// </summary>
    [DefaultValue(null)]
    public bool? IsActive { get; set; }
}
