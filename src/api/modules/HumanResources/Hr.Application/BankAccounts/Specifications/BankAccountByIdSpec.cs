namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

using Ardalis.Specification;
using Domain.Entities;

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

