using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;

public sealed class LegalActionByIdSpec : Specification<LegalAction>, ISingleResultSpecification<LegalAction>
{
    public LegalActionByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
