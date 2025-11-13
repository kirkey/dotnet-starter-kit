using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

public class SearchEmployeeDocumentsSpec : EntitiesByPaginationFilterSpec<EmployeeDocument, EmployeeDocumentResponse>
{
    public SearchEmployeeDocumentsSpec(SearchEmployeeDocumentsRequest request)
        : base(request) =>
        Query
            .Where(d => d.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(d => d.DocumentType == request.DocumentType, !string.IsNullOrWhiteSpace(request.DocumentType))
            .Where(d => d.IsExpired == request.IsExpired, request.IsExpired.HasValue)
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderByDescending(d => d.UploadedDate, !request.HasOrderBy());
}

