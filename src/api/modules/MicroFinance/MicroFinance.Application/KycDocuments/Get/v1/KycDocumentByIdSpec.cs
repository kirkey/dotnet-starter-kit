using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;

public sealed class KycDocumentByIdSpec : Specification<KycDocument>, ISingleResultSpecification<KycDocument>
{
    public KycDocumentByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
