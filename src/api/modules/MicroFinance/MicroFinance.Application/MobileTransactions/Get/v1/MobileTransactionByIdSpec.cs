using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

public sealed class MobileTransactionByIdSpec : Specification<MobileTransaction>, ISingleResultSpecification<MobileTransaction>
{
    public MobileTransactionByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
