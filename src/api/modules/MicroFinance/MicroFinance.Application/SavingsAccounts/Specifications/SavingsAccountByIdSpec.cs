using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;

public sealed class SavingsAccountByIdSpec : Specification<SavingsAccount>, ISingleResultSpecification<SavingsAccount>
{
    public SavingsAccountByIdSpec(Guid id)
    {
        Query.Where(sa => sa.Id == id)
            .Include(sa => sa.Member)
            .Include(sa => sa.SavingsProduct)
            .Include(sa => sa.Transactions);
    }
}
