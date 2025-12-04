using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;

public sealed class DebtSettlementByIdSpec : Specification<DebtSettlement>, ISingleResultSpecification<DebtSettlement>
{
    public DebtSettlementByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
