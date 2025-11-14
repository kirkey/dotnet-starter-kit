namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting bank account by ID.
/// </summary>
public sealed class BankAccountByIdSpec : Specification<BankAccount>, ISingleResultSpecification<BankAccount>
{
    public BankAccountByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching bank accounts with filters.
/// </summary>
public sealed class SearchBankAccountsSpec : Specification<BankAccount>
{
    public SearchBankAccountsSpec(SearchBankAccountsRequest request)
    {
        Query.OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.IsActive)
            .ThenBy(x => x.BankName)
            .Include(x => x.Employee);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.BankName))
            Query.Where(x => x.BankName.Contains(request.BankName));

        if (!string.IsNullOrWhiteSpace(request.AccountType))
            Query.Where(x => x.AccountType == request.AccountType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.IsPrimary.HasValue)
            Query.Where(x => x.IsPrimary == request.IsPrimary.Value);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

/// <summary>
/// Specification for getting employee's primary bank account.
/// </summary>
public sealed class PrimaryBankAccountByEmployeeSpec : Specification<BankAccount>, ISingleResultSpecification<BankAccount>
{
    public PrimaryBankAccountByEmployeeSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.IsPrimary && x.IsActive)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for getting all active accounts by employee.
/// </summary>
public sealed class ActiveBankAccountsByEmployeeSpec : Specification<BankAccount>
{
    public ActiveBankAccountsByEmployeeSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.BankName)
            .Include(x => x.Employee);
    }
}

