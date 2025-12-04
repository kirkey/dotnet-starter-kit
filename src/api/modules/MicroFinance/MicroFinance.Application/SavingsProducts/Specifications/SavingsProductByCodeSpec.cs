using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;

public sealed class SavingsProductByCodeSpec : Specification<SavingsProduct>, ISingleResultSpecification<SavingsProduct>
{
    public SavingsProductByCodeSpec(string code)
    {
        Query.Where(sp => sp.Code == code);
    }
}
