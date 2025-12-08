using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Get.v1;

public sealed class CreditBureauReportByIdSpec : Specification<CreditBureauReport>
{
    public CreditBureauReportByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
