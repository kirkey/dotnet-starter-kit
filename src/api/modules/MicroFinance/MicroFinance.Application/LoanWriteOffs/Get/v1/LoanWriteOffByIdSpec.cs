using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed class LoanWriteOffByIdSpec : Specification<LoanWriteOff>, ISingleResultSpecification<LoanWriteOff>
{
    public LoanWriteOffByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
