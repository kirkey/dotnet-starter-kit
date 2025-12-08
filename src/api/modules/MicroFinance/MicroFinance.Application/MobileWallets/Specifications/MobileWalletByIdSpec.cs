using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;

public sealed class MobileWalletByIdSpec : Specification<MobileWallet>, ISingleResultSpecification<MobileWallet>
{
    public MobileWalletByIdSpec(DefaultIdType id) => Query.Where(x => x.Id == id);
}
