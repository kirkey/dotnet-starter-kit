using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Specifications;

public sealed class LoanCollateralByIdSpec : Specification<LoanCollateral>, ISingleResultSpecification<LoanCollateral>
{
    public LoanCollateralByIdSpec(Guid id)
    {
        Query.Where(lc => lc.Id == id);
    }
}
