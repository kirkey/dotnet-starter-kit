using Accounting.Domain.Entities;

namespace Accounting.Application.AccountsReceivableAccounts.Queries;

/// <summary>
/// Specification to find accounts receivable account by account number.
/// </summary>
public class AccountsReceivableAccountByNumberSpec : Specification<AccountsReceivableAccount>
{
    public AccountsReceivableAccountByNumberSpec(string accountNumber)
    {
        Query.Where(a => a.AccountNumber == accountNumber);
    }
}

/// <summary>
/// Specification to find accounts receivable account by ID.
/// </summary>
public class AccountsReceivableAccountByIdSpec : Specification<AccountsReceivableAccount>
{
    public AccountsReceivableAccountByIdSpec(DefaultIdType id)
    {
        Query.Where(a => a.Id == id);
    }
}

/// <summary>
/// Specification for searching accounts receivable accounts with filters.
/// </summary>
public class AccountsReceivableAccountSearchSpec : Specification<AccountsReceivableAccount>
{
    public AccountsReceivableAccountSearchSpec(
        string? accountNumber = null,
        string? accountName = null,
        bool? isReconciled = null,
        decimal? minBalance = null,
        decimal? maxBalance = null)
    {
        if (!string.IsNullOrWhiteSpace(accountNumber))
        {
            Query.Where(a => a.AccountNumber.Contains(accountNumber));
        }

        if (!string.IsNullOrWhiteSpace(accountName))
        {
            Query.Where(a => a.AccountName.Contains(accountName));
        }

        if (isReconciled.HasValue)
        {
            Query.Where(a => a.IsReconciled == isReconciled.Value);
        }

        if (minBalance.HasValue)
        {
            Query.Where(a => a.CurrentBalance >= minBalance.Value);
        }

        if (maxBalance.HasValue)
        {
            Query.Where(a => a.CurrentBalance <= maxBalance.Value);
        }

        Query.OrderBy(a => a.AccountNumber);
    }
}

