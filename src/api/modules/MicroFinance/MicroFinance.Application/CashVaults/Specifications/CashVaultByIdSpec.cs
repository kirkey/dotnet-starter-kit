using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Specifications;

public sealed class CashVaultByIdSpec : Specification<CashVault>, ISingleResultSpecification<CashVault>
{
    public CashVaultByIdSpec(DefaultIdType id)
    {
        Query.Where(v => v.Id == id);
    }
}
