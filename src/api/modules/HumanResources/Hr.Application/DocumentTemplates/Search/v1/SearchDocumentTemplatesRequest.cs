using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Search.v1;

public class SearchDocumentTemplatesRequest : PaginationFilter, IRequest<PagedList<DocumentTemplateResponse>>
{
    public string? SearchString { get; set; }
    public string? DocumentType { get; set; }
    public bool? IsActive { get; set; }
}

