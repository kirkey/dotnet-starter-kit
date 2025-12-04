using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;

public sealed class ReportDefinitionByIdSpec : Specification<ReportDefinition>
{
    public ReportDefinitionByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
