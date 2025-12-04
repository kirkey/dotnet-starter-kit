using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;

public sealed class SavingsAccountByAccountNumberSpec : Specification<SavingsAccount>, ISingleResultSpecification<SavingsAccount>
{
    public SavingsAccountByAccountNumberSpec(string accountNumber)
    {
        Query.Where(sa => sa.AccountNumber == accountNumber);
    }
}
