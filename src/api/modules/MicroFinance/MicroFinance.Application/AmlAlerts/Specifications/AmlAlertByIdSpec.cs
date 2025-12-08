using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;

public sealed class AmlAlertByIdSpec : Specification<AmlAlert>, ISingleResultSpecification<AmlAlert>
{
    public AmlAlertByIdSpec(DefaultIdType id) => Query.Where(x => x.Id == id);
}
