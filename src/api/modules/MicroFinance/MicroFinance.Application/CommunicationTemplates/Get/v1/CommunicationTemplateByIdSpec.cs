using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;

public sealed class CommunicationTemplateByIdSpec : Specification<CommunicationTemplate>, ISingleResultSpecification<CommunicationTemplate>
{
    public CommunicationTemplateByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
