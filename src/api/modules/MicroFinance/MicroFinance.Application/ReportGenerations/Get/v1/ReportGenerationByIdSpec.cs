using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;

public sealed class ReportGenerationByIdSpec : Specification<ReportGeneration>
{
    public ReportGenerationByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
