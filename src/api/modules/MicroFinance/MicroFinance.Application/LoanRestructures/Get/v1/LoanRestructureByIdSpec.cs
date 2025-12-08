using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;

public sealed class LoanRestructureByIdSpec : Specification<LoanRestructure>, ISingleResultSpecification<LoanRestructure>
{
    public LoanRestructureByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
