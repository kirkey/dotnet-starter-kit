using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;

public sealed class LoanProductByIdSpec : Specification<LoanProduct>, ISingleResultSpecification<LoanProduct>
{
    public LoanProductByIdSpec(Guid id)
    {
        Query.Where(lp => lp.Id == id);
    }
}
