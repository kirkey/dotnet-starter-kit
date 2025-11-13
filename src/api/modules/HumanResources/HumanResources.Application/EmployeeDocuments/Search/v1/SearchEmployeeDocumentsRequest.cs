using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

public class SearchEmployeeDocumentsRequest : PaginationFilter, IRequest<PagedList<EmployeeDocumentResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public string? DocumentType { get; set; }
    public bool? IsExpired { get; set; }
    public bool? IsActive { get; set; }
}

