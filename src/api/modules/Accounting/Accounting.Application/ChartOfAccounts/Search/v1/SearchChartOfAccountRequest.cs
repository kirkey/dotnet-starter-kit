using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

/// <summary>
/// Request to search chart of accounts with filtering and pagination.
/// </summary>
public class SearchChartOfAccountRequest : PaginationFilter, IRequest<PagedList<ChartOfAccountResponse>>
{
    /// <summary>
    /// Filter by account code.
    /// </summary>
    public string? AccountCode { get; set; }
    
    /// <summary>
    /// Filter by account name.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Filter by description.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Filter by notes.
    /// </summary>
    public string? Notes { get; set; }
}
