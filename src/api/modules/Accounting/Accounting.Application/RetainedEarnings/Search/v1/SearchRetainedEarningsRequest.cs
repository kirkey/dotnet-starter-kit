using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Search.v1;

/// <summary>
/// Request to search for retained earnings with optional filters and pagination.
/// </summary>
public class SearchRetainedEarningsRequest : PaginationFilter, IRequest<PagedList<RetainedEarningsResponse>>
{
    /// <summary>
    /// Filter by specific fiscal year.
    /// </summary>
    public int? FiscalYear { get; set; }
    
    /// <summary>
    /// Filter by status (Open, Closed, Locked).
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Filter to show only closed years.
    /// </summary>
    public bool? IsClosed { get; set; }
    
    /// <summary>
    /// Filter to show only open years (not closed).
    /// </summary>
    public bool OnlyOpen { get; set; }
}

