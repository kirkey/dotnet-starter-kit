namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

using Ardalis.Specification;
using Domain.Entities;

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

