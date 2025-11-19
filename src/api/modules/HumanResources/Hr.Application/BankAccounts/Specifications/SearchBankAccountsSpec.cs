namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

using Ardalis.Specification;
using Search.v1;
using Domain.Entities;

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

