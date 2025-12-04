using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Specifications;

public sealed class InvestmentTransactionByIdSpec : Specification<InvestmentTransaction>, ISingleResultSpecification<InvestmentTransaction>
{
    public InvestmentTransactionByIdSpec(Guid id) => Query.Where(x => x.Id == id);
}
