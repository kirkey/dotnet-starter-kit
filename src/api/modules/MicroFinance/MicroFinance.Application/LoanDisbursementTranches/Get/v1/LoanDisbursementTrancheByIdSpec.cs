using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed class LoanDisbursementTrancheByIdSpec : Specification<LoanDisbursementTranche>, ISingleResultSpecification<LoanDisbursementTranche>
{
    public LoanDisbursementTrancheByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
