using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;

public sealed class ShareProductByCodeSpec : Specification<ShareProduct>, ISingleResultSpecification<ShareProduct>
{
    public ShareProductByCodeSpec(string code)
    {
        Query.Where(sp => sp.Code == code);
    }
}
