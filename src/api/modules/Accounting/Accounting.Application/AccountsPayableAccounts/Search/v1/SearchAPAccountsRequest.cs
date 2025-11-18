using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Search.v1;

/// <summary>
/// Request to search for accounts payable accounts with optional filters and pagination.
/// </summary>
public class SearchApAccountsRequest : PaginationFilter, IRequest<PagedList<ApAccountResponse>>
{
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public bool? IsReconciled { get; set; }
}

