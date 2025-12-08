using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;

public sealed class ShareAccountByIdSpec : Specification<ShareAccount>, ISingleResultSpecification<ShareAccount>
{
    public ShareAccountByIdSpec(DefaultIdType id)
    {
        Query.Where(sa => sa.Id == id);
    }
}
