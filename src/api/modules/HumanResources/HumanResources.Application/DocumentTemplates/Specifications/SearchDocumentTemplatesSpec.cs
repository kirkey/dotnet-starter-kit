using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Specifications;

public class SearchDocumentTemplatesSpec : EntitiesByPaginationFilterSpec<DocumentTemplate, DocumentTemplateResponse>
{
    public SearchDocumentTemplatesSpec(SearchDocumentTemplatesRequest request)
        : base(request) =>
        Query
            .Where(d => (d.TemplateName.Contains(request.SearchString!) || d.Description!.Contains(request.SearchString!)), !string.IsNullOrWhiteSpace(request.SearchString))
            .Where(d => d.DocumentType == request.DocumentType, !string.IsNullOrWhiteSpace(request.DocumentType))
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderBy(d => d.TemplateName, !request.HasOrderBy());
}

