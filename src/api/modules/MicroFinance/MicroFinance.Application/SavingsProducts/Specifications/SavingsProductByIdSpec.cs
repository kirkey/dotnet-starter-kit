using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;

public sealed class SavingsProductByIdSpec : Specification<SavingsProduct>, ISingleResultSpecification<SavingsProduct>
{
    public SavingsProductByIdSpec(Guid id)
    {
        Query.Where(sp => sp.Id == id);
    }
}
