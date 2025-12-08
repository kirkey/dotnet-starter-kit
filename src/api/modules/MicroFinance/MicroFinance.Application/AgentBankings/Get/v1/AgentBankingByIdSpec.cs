using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;

public sealed class AgentBankingByIdSpec : Specification<AgentBanking>
{
    public AgentBankingByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
