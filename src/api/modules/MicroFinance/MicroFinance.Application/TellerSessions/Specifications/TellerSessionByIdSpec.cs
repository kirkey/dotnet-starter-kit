using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;

public sealed class TellerSessionByIdSpec : Specification<TellerSession>, ISingleResultSpecification<TellerSession>
{
    public TellerSessionByIdSpec(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);
    }
}
