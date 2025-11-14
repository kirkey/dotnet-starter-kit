namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

/// <summary>
/// Specification for getting a bank account by ID.
/// </summary>
public class BankAccountByIdSpec : Specification<BankAccount>, ISingleResultSpecification<BankAccount>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankAccountByIdSpec"/> class.
    /// </summary>
    public BankAccountByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching bank accounts with filters.
/// </summary>
public class SearchBankAccountsSpec : Specification<BankAccount>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchBankAccountsSpec"/> class.
    /// </summary>
    public SearchBankAccountsSpec(Search.v1.SearchBankAccountsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderBy(x => x.IsPrimary)
            .ThenBy(x => x.BankName);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.AccountType))
            Query.Where(x => x.AccountType == request.AccountType);

        if (!string.IsNullOrWhiteSpace(request.BankName))
            Query.Where(x => x.BankName.Contains(request.BankName));

        if (request.IsPrimary.HasValue)
            Query.Where(x => x.IsPrimary == request.IsPrimary);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsVerified.HasValue)
            Query.Where(x => x.IsVerified == request.IsVerified);
    }
}

