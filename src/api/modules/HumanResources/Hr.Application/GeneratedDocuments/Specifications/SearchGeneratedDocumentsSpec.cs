using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Specifications;

public class SearchGeneratedDocumentsSpec : EntitiesByPaginationFilterSpec<GeneratedDocument, GeneratedDocumentResponse>
{
    public SearchGeneratedDocumentsSpec(SearchGeneratedDocumentsRequest request)
        : base(request) =>
        Query
            .Where(d => d.EntityId == request.EntityId, request.EntityId.HasValue)
            .Where(d => d.EntityType == request.EntityType, !string.IsNullOrWhiteSpace(request.EntityType))
            .Where(d => d.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderByDescending(d => d.GeneratedDate, !request.HasOrderBy());
}

