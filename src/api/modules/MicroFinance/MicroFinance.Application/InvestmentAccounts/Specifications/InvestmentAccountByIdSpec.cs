using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Specifications;

public sealed class InvestmentAccountByIdSpec : Specification<InvestmentAccount>, ISingleResultSpecification<InvestmentAccount>
{
    public InvestmentAccountByIdSpec(Guid id) => Query.Where(x => x.Id == id);
}
