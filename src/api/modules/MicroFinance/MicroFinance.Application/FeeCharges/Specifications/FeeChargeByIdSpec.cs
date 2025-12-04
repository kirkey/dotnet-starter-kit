using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Specifications;

public sealed class FeeChargeByIdSpec : Specification<FeeCharge>, ISingleResultSpecification<FeeCharge>
{
    public FeeChargeByIdSpec(Guid id)
    {
        Query.Where(fc => fc.Id == id);
    }
}
