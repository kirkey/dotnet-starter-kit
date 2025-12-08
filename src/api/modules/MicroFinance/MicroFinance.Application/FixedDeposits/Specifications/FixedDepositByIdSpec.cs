using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Specifications;

public sealed class FixedDepositByIdSpec : Specification<FixedDeposit>, ISingleResultSpecification<FixedDeposit>
{
    public FixedDepositByIdSpec(DefaultIdType id)
    {
        Query.Where(fd => fd.Id == id);
    }
}
