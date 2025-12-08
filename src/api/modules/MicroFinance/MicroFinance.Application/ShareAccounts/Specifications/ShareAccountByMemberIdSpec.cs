using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;

public sealed class ShareAccountByMemberIdSpec : Specification<ShareAccount>
{
    public ShareAccountByMemberIdSpec(DefaultIdType memberId)
    {
        Query.Where(sa => sa.MemberId == memberId);
    }
}
