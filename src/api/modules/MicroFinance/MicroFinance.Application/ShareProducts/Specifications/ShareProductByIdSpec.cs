using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;

public sealed class ShareProductByIdSpec : Specification<ShareProduct>, ISingleResultSpecification<ShareProduct>
{
    public ShareProductByIdSpec(DefaultIdType id)
    {
        Query.Where(sp => sp.Id == id);
    }
}
