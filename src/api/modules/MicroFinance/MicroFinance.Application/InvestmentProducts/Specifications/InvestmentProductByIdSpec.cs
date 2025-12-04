using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;

public sealed class InvestmentProductByIdSpec : Specification<InvestmentProduct>, ISingleResultSpecification<InvestmentProduct>
{
    public InvestmentProductByIdSpec(Guid id) => Query.Where(x => x.Id == id);
}
