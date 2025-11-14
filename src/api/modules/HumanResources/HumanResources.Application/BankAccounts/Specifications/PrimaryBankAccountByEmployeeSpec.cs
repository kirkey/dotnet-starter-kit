namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

using Ardalis.Specification;
using Domain.Entities;

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

