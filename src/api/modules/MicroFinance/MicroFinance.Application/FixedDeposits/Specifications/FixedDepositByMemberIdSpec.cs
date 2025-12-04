using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Specifications;

public sealed class FixedDepositByMemberIdSpec : Specification<FixedDeposit>
{
    public FixedDepositByMemberIdSpec(Guid memberId)
    {
        Query.Where(fd => fd.MemberId == memberId);
    }
}
