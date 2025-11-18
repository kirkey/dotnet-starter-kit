namespace Accounting.Application.AccountsPayableAccounts.Queries;

/// <summary>
/// Specification to find accounts payable account by account number.
/// </summary>
public class AccountsPayableAccountByNumberSpec : Specification<AccountsPayableAccount>
{
    public AccountsPayableAccountByNumberSpec(string accountNumber)
    {
        Query.Where(a => a.AccountNumber == accountNumber);
    }
}

/// <summary>
/// Specification to find accounts payable account by ID.
/// </summary>
public class AccountsPayableAccountByIdSpec : Specification<AccountsPayableAccount>
{
    public AccountsPayableAccountByIdSpec(DefaultIdType id)
    {
        Query.Where(a => a.Id == id);
    }
}

/// <summary>
/// Specification for searching accounts payable accounts with filters and pagination.
/// </summary>
public class AccountsPayableAccountSearchSpec : EntitiesByPaginationFilterSpec<AccountsPayableAccount, Responses.ApAccountResponse>
{
    public AccountsPayableAccountSearchSpec(Search.v1.SearchApAccountsRequest request)
        : base(request)
    {
        if (!string.IsNullOrWhiteSpace(request.AccountNumber))
        {
            Query.Where(a => a.AccountNumber.Contains(request.AccountNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.AccountName))
        {
            Query.Where(a => a.AccountName.Contains(request.AccountName));
        }

        if (request.IsReconciled.HasValue)
        {
            Query.Where(a => a.IsReconciled == request.IsReconciled.Value);
        }

        Query.OrderBy(a => a.AccountNumber);
    }
}

