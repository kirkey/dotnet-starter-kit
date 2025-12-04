using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;

public sealed class LoanProductByCodeSpec : Specification<LoanProduct>, ISingleResultSpecification<LoanProduct>
{
    public LoanProductByCodeSpec(string code)
    {
        Query.Where(lp => lp.Code == code);
    }
}
