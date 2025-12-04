using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;

public sealed class LoansByProductIdSpec : Specification<Loan>
{
    public LoansByProductIdSpec(Guid productId)
    {
        Query.Where(l => l.LoanProductId == productId);
    }
}
