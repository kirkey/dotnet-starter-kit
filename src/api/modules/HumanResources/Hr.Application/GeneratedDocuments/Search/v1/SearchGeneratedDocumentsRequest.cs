using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Search.v1;

public class SearchGeneratedDocumentsRequest : PaginationFilter, IRequest<PagedList<GeneratedDocumentResponse>>
{
    public DefaultIdType? EntityId { get; set; }
    public string? EntityType { get; set; }
    public string? Status { get; set; }
    public bool? IsActive { get; set; }
}

