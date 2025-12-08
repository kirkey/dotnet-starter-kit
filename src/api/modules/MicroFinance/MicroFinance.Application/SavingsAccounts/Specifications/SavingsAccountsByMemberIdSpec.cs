using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;

public sealed class SavingsAccountsByMemberIdSpec : Specification<SavingsAccount>
{
    public SavingsAccountsByMemberIdSpec(DefaultIdType memberId, bool? activeOnly = null)
    {
        Query.Where(sa => sa.MemberId == memberId);

        if (activeOnly == true)
        {
            Query.Where(sa => sa.Status == SavingsAccount.StatusActive);
        }
    }
}
